using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using APP_PELIS.Models;
using APP_PELIS.Service;

namespace APP_PELIS.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly UserManager<Usuario> _userManager;
        private readonly SignInManager<Usuario> _signInManager;
        private readonly ImagenStorage _imagenStorage;
        private readonly IEmailService _emailService;
        public UsuarioController(UserManager<Usuario> userManager, SignInManager<Usuario> signInManager, ImagenStorage imagenStorage, IEmailService emailService)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _imagenStorage = imagenStorage;
            _emailService = emailService;
        }
        //GET OLVIDE CONTRASEÑA
        public IActionResult OlvideContraseña()
        {
            return View();
        }
        //POST OLVIDE CONTRASEÑA
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OlvideContraseña(OlvideContraseñaViewModel modelo)
        {
            if (ModelState.IsValid)
            {
                var usuario = await _userManager.FindByEmailAsync(modelo.Email);
                if (usuario != null)
                {
                    var token = await _userManager.GeneratePasswordResetTokenAsync(usuario);
                    var enlaceReset = Url.Action("ResetearContraseña", "Usuario", new { email = modelo.Email, token = token }, Request.Scheme);
                    var contenidoEmail = $"<h1>Solicitud de reseteo de contraseña</h1>" +
                                        $"<p>Haga clic en el siguiente enlace para resetear su contraseña:</p>" +
                                        $"<a href='{enlaceReset}'>Resetear Contraseña</a>";
                    await _emailService.SendAsync(modelo.Email, "Resetear Contraseña", contenidoEmail);
                }
                // Siempre mostrar el mismo mensaje para evitar revelar si el email existe o no
                ViewBag.Mensaje = "Si el email existe en nuestro sistema, se ha enviado un enlace para resetear la contraseña.";
                return View();
            }
            return View(modelo);
        }
        //GET RESETEAR CONTRASEÑA
        public IActionResult ResetearContraseña(string email, string token)
        {
            if (token == null || email == null) return RedirectToAction("Login");

            var modelo = new ResetearContraseñaViewModel { Email = email, Token = token };
            return View(modelo);
        }
        //POST RESETEAR CONTRASEÑA
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetearContraseña(ResetearContraseñaViewModel modelo)
        {
            if (ModelState.IsValid)
            {
                var usuario = await _userManager.FindByEmailAsync(modelo.Email);
                if (usuario != null)
                {
                    var resultado = await _userManager.ResetPasswordAsync(usuario, modelo.Token, modelo.NuevaClave);
                    if (resultado.Succeeded)
                    {
                        TempData["Mensaje"] = "Contraseña restablecida con éxito.";
                        return View();
                    }
                    else
                    {
                        foreach (var error in resultado.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                    }
                }
                else
                {
                    // No revelar que el usuario no existe
                    ViewBag.Mensaje = "Si el email existe en nuestro sistema, se ha reseteado la contraseña.";
                    return View();
                }
            }
            return View(modelo);
        }
        //GET LOGIN
        public IActionResult Login()
        {
            return View();
        }
        //POST LOGIN
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel usuario)
        {
            if (ModelState.IsValid)
            {
                var resultado = await _signInManager.PasswordSignInAsync(
                    usuario.Email, 
                    usuario.Clave, 
                    usuario.Recordarme, 
                    lockoutOnFailure: false);
                if (resultado.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Inicio de sesion invalido.");
                }
            }


            return View(usuario);
        }

        public IActionResult Registro()
        {
            return View();
        }

        //POST REGISTRO
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Registro(RegistroViewModel usuario)
        {
            if (ModelState.IsValid)
            {
                var nuevoUsuario = new Usuario
                {
                    UserName = usuario.Email,
                    Email = usuario.Email,
                    Nombre = usuario.Nombre,
                    Apellido = usuario.Apellido,
                    ImagenPerfilUrl = "/images/default-avatar.png"
                };
                var resultado = await _userManager.CreateAsync(nuevoUsuario, usuario.Clave);

                if (resultado.Succeeded)
                {
                    string cuerpoEmail = _emailService.MensajeBienvenida(nuevoUsuario.Nombre);
                    await _signInManager.SignInAsync(nuevoUsuario, isPersistent: false);
                    await _emailService.SendAsync(nuevoUsuario.Email, "Bienvenido/a a Rpelis", cuerpoEmail);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    foreach (var error in resultado.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }

            return View(usuario);
        }
        [HttpPost]
        public  IActionResult Logout()
        {
             _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
        [Authorize]
        public async Task<IActionResult> MiPerfil()
        {
            var usuarioActual = await _userManager.GetUserAsync(User);
         
            var usuarioVM = new MiPerfilViewModel
            {
                Nombre = usuarioActual.Nombre,
                Apellido = usuarioActual.Apellido,
                Email = usuarioActual.Email,
                ImagenPerfilUrl = usuarioActual.ImagenPerfilUrl
            };
            return View(usuarioVM);
        }
        //POST MI PERFIL
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MiPerfil(MiPerfilViewModel usuarioVM)
        {
            if (ModelState.IsValid)
            {
                var usuarioActual = await _userManager.GetUserAsync(User);

                try
                {
                    if (usuarioVM.ImagenPerfil is not null && usuarioVM.ImagenPerfil.Length > 0)
                    {
                        if (!string.IsNullOrWhiteSpace(usuarioActual.ImagenPerfilUrl))
                            await _imagenStorage.DeleteAsync(usuarioActual.ImagenPerfilUrl);

                        var nuevaRuta = await _imagenStorage.SaveAsync(usuarioActual.Id, usuarioVM.ImagenPerfil);
                        usuarioActual.ImagenPerfilUrl = nuevaRuta;
                        usuarioVM.ImagenPerfilUrl = nuevaRuta;
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                    return View(usuarioVM);
                }

                usuarioActual.Nombre = usuarioVM.Nombre;
                usuarioActual.Apellido = usuarioVM.Apellido;

                var resultado = await _userManager.UpdateAsync(usuarioActual);

                if (resultado.Succeeded)
                {
                    ViewBag.Mensaje = "Perfil actualizado correctamente.";
                    usuarioVM.Email = usuarioActual.Email;

                    if (string.IsNullOrEmpty(usuarioVM.ImagenPerfilUrl))
                    {
                        usuarioVM.ImagenPerfilUrl = usuarioActual.ImagenPerfilUrl;
                    }
                    return View(usuarioVM);
                }
                else
                {
                    foreach (var error in resultado.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }

            }

            return View(usuarioVM);
        }

    }
}
