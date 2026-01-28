using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace APP_PELIS.Models
{
    public class Usuario : IdentityUser
    {
        [Required]
        [StringLength(50)]
        public string Nombre { get; set; }
        [Required]
        [StringLength(50)]
        public string Apellido { get; set; }
        [DataType(DataType.Date)]
        public DateTime FechaNacimiento { get; set; }
        public string ImagenPerfilUrl { get; set; }
        public List<Favorito>? PeliculasFavoritas { get; set; }
        public List<Review>? ReviewsUsuario { get; set; }

    }

    public class RegistroViewModel
    {
        [Required(ErrorMessage = "Ingrese un nombre.")]
        [StringLength(50)]
        public string Nombre { get; set; }
        [Required(ErrorMessage = "Ingrese el apellido.")]
        [StringLength(50)]
        public string Apellido { get; set; }
        [EmailAddress(ErrorMessage = "Ingrese un email valido.")]
        [Required(ErrorMessage = "El email es obligatorio.")]
        public string Email { get; set; }
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "La clave es obligatoria.")]
        public string Clave { get; set; }
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Debes confirmar la clave.")]
        [Compare("Clave", ErrorMessage = "Las claves no coinciden.")]
        public string ConfirmarClave { get; set; }
    }
    public class LoginViewModel
    {
        [EmailAddress(ErrorMessage = "Ingrese un email valido.")]
        [Required(ErrorMessage = "El email es obligatorio.")]
        public string Email { get; set; }
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "La clave es obligatoria.")]
        public string Clave { get; set; }
        public bool Recordarme { get; set; }
    }

    public class MiPerfilViewModel
    {
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string? Email { get; set; }
        public IFormFile? ImagenPerfil { get; set; }
        public string? ImagenPerfilUrl { get; set; }
    }
    public class OlvideContraseñaViewModel
    {
        [Required(ErrorMessage = "El correo electrónico es obligatorio")]
        [EmailAddress(ErrorMessage = "Ingrese un correo electrónico válido")]
        [Display(Name = "Correo Electrónico")]
        public string Email { get; set; }
    }
    public class ResetearContraseñaViewModel
    {
        [Required]
        public string Token { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "La nueva contraseña es obligatoria")]
        [StringLength(100, ErrorMessage = "La {0} debe tener al menos {2} caracteres.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Nueva Contraseña")]
        public string NuevaClave { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirmar Nueva Contraseña")]
        [Compare("NuevaClave", ErrorMessage = "La contraseña y la confirmación no coinciden.")]
        public string ConfirmarClave { get; set; }
    }
}
