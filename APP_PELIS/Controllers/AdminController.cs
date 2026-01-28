using APP_PELIS.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APP_PELIS.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly MovieDbContext _context;
        public AdminController(MovieDbContext context)
        {
            _context = context;
        }
        public async Task <IActionResult> Index()
        {
            
            var mejorPelicula = await _context.Peliculas
                .OrderByDescending(p => p.ListaReviews.Average(r => r.Rating))
                .FirstOrDefaultAsync();

            var totalUsuarios = await _context.Users.CountAsync();


            var totalResenas = await _context.Reviews.CountAsync();


            var generoPopular = await _context.Generos
                .OrderByDescending(g => g.PeliculasGenero.Count())
                .Select(g => g.Descripcion)
                .FirstOrDefaultAsync();

            ViewBag.MejorPelicula = mejorPelicula?.Titulo ?? "N/A";
            ViewBag.TotalUsuarios = totalUsuarios;
            ViewBag.TotalResenas = totalResenas;
            ViewBag.GeneroPopular = generoPopular;

            var datosGrafico = await _context.Generos
                .Select(g => new {
                Nombre = g.Descripcion,
                Cantidad = g.PeliculasGenero.Count()
                })
                .Where(g => g.Cantidad > 0) // Solo géneros que tengan pelis
                .ToListAsync();

            ViewBag.NombresGeneros = datosGrafico.Select(g => g.Nombre).ToList();
            ViewBag.CantidadesGeneros = datosGrafico.Select(g => g.Cantidad).ToList();


            return View();
        }
    }
}
