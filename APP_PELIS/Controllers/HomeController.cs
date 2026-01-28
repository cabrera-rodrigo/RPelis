using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System.Diagnostics;
using System.Security.Claims;
using APP_PELIS.Data;
using APP_PELIS.Models;

namespace APP_PELIS.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly MovieDbContext _context;
        private readonly UserManager<Usuario> _userManager;

        public HomeController(ILogger<HomeController> logger, MovieDbContext context, UserManager<Usuario> userManager)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
        }
        public async Task<IActionResult> Details(int id)
        {
            var pelicula = await _context.Peliculas
                .Include(p => p.Genero)
                .Include(p => p.ListaReviews)
                .ThenInclude(r => r.Usuario)
                .FirstOrDefaultAsync(p => p.Id == id);

            //LOGICA DE PELICULAS RELACIONADAS
            var relacionadas = await _context.Peliculas
                .Where(p => p.GeneroId == pelicula.GeneroId && p.Id != id)
                .OrderBy(p=> Guid.NewGuid())
                .Take(4)
                .ToListAsync();
            ViewBag.Relacionadas = relacionadas; 

            ViewBag.UserReview = false;
            if (User?.Identity?.IsAuthenticated == true && pelicula.ListaReviews != null)
            {
                string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                ViewBag.UserReview = !(pelicula.ListaReviews.FirstOrDefault(r => r.UsuarioId == userId) == null);
            }

            return View(pelicula);
        }
        public async Task<IActionResult> Index(int page = 1, string txtBusqueda = "", int generoId = 0, string ordenarPor = "")
        {
            const int pageSize = 8;

            if (page < 1) page = 1;

            var consulta = _context.Peliculas
                .Include(p=>p.ListaReviews)
                .AsQueryable(); // Consulta base
            if (!string.IsNullOrEmpty(txtBusqueda)) // Si hay texto de búsqueda
            {
                consulta = consulta.Where(p => p.Titulo.Contains(txtBusqueda)); // Filta por titulo
            } 

            if (generoId > 0)
            {
                consulta = consulta.Where(p => p.GeneroId == generoId); // Filtra por género                        
            }
            // LÓGICA DE ORDENAMIENTO 
            switch (ordenarPor)
            {
                case "rating_desc":
                    consulta = consulta.OrderByDescending(p => p.ListaReviews.Select(r => (double?)r.Rating).Average() ?? 0);
                    break;
                case "rating_asc":
                    consulta = consulta.OrderBy(p => p.ListaReviews.Select(r => (double?)r.Rating).Average() ?? 0);
                    break;
                case "titulo_desc":
                    consulta = consulta.OrderByDescending(p => p.Titulo);
                    break;
                default:
                    consulta = consulta.OrderBy(p => p.Titulo);
                    break;
            }
            //PAGINACION
            var totalCount = await consulta.CountAsync(); // Cuenta las peliculas que cumplen el filtro de busqueda
            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
            if (totalPages == 0) totalPages = 1;
            if (page > totalPages) page = totalPages;

            var peliculas = await consulta // Usa la consulta filtrada
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            //   LÓGICA DE FAVORITOS 
            List<int> favoritosIds = new List<int>();
            if (User.Identity.IsAuthenticated)
            {
                var usuarioId = _userManager.GetUserId(User);
                favoritosIds = await _context.Favoritos
                    .Where(f => f.UsuarioId == usuarioId)
                    .Select(f => f.PeliculaId)
                    .ToListAsync();
            }
            ViewBag.FavoritosIds = favoritosIds;

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;
            ViewBag.TxtBusqueda = txtBusqueda; // Mantener el texto de búsqueda en la vista
            ViewBag.SelectedGeneroId = generoId;
            ViewBag.CurrentSort = ordenarPor;
            

            var generos = await _context.Generos
                .OrderBy(g => g.Descripcion)
                .ToListAsync(); // Obtener la lista de géneros
            generos.Insert(0, new Genero { Id = 0, Descripcion = "Todas" }); // Opcion para todos los géneros
            ViewBag.GeneroId = new SelectList(
                generos,
                "Id",
                "Descripcion",
                generoId
                ); // Seleccionar el género actual
                   // Usamos .Include para traer las reseñas de cada película
            

            return View(peliculas);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
