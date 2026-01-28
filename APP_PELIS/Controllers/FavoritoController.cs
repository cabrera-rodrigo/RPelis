using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using APP_PELIS.Data;
using APP_PELIS.Models;

namespace APP_PELIS.Controllers
{
    [Authorize]
    public class FavoritoController : Controller
    {
        private readonly MovieDbContext _context;
        private readonly UserManager<Usuario> _userManager;
        public FavoritoController(MovieDbContext context, UserManager<Usuario> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        // GET: FavoritoController
        public async Task<ActionResult> Index()
        {
            var usuarioId = _userManager.GetUserId(User);

            var misFavoritos = await _context.Favoritos
                .Where(f => f.UsuarioId == usuarioId)
                .Include(f => f.Pelicula)
                .OrderByDescending(f => f.Fecha)
                .Select(f => f.Pelicula)
                .ToListAsync();

            return View(misFavoritos);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AgregarFavorito(int peliculaId)
        {
            var usuarioId = _userManager.GetUserId(User);
            var favoritoExistente = await _context.Favoritos
                .FirstOrDefaultAsync(f => f.UsuarioId == usuarioId && f.PeliculaId == peliculaId);
            if (favoritoExistente != null)
            {
                _context.Favoritos.Remove(favoritoExistente);
            }
            else
            {
                var nuevoFavorito = new Favorito
                {
                    UsuarioId = usuarioId,
                    PeliculaId = peliculaId,
                    Fecha = DateTime.Now
                };
                _context.Favoritos.Add(nuevoFavorito);
            }

            await _context.SaveChangesAsync();

            return Redirect(Request.Headers["Referer"].ToString() ?? "/");
            //return RedirectToAction("Index");
        }
        public async Task<ActionResult> EliminarFavorito(int peliculaId)
        {
            var usuarioId = _userManager.GetUserId(User);
            var favorito = await _context.Favoritos
                .FirstOrDefaultAsync(f => f.UsuarioId == usuarioId && f.PeliculaId == peliculaId);
            if(favorito != null)
            {
                _context.Favoritos.Remove(favorito);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }

    }
}
