using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection.Metadata.Ecma335;

namespace APP_PELIS.Models
{
    public class Pelicula
    {
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string Titulo { get; set; }
        [DataType(DataType.Date)]
        public DateTime FechaLanzamiento { get; set; }
        [Required]
        [Range(1,500)]
        public int MinutosDuracion { get; set; }
        [Required]
        [StringLength(1000)]
        public string Sinopsis { get; set; }
        [Url]
        [Required]
        public string PosterUrl { get; set; }
        public int GeneroId { get; set; }
        public Genero? Genero { get; set; }
        public int PlataformaId { get; set; }
        public Plataforma? Plataforma { get; set; }
        [NotMapped]
        public int PromedioRating { get; set; }
        public List<Review>? ListaReviews { get; set; }
        public List<Favorito>? UsuariosFavorito { get; set; }
    }
}
