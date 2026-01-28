using System.ComponentModel.DataAnnotations;

namespace APP_PELIS.Models
{
    public class Review
    {
        public int Id { get; set; }
        public int PeliculaId { get; set; }
        public Pelicula? Pelicula { get; set; }
        public string UsuarioId { get; set; }
        public Usuario? Usuario { get; set; }
        [Range(1,5)]
        public int Rating { get; set; }
        [Required]
        [StringLength(500)]
        public string Comentario { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime FechaReview { get; set; }
        [Timestamp]
        public byte[] RowVersion { get; set; }
    }

    public class ReviewCreateViewModel
    {
        public int? Id { get; set; }
        public int PeliculaId { get; set; }
        public string? PeliculaTitulo { get; set; }
        public string UsuarioId { get; set; } = string.Empty;
        [Range(1, 5, ErrorMessage = "La calificacion debe ser entre 1 y 5 estrellas")]
        [Required(ErrorMessage = "La calificacion es obligatoria")]
        public int Rating { get; set; }
        [StringLength(500, ErrorMessage = "El comentario no puede exceder los 500 caracteres")]
        [Required(ErrorMessage = "El comentario es obligatorio")]
        public string Comentario { get; set; } = string.Empty;
    }
}
