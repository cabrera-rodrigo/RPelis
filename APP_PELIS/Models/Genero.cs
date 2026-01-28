using System.ComponentModel.DataAnnotations;

namespace APP_PELIS.Models
{
    public class Genero
    {
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string Descripcion { get; set; }
        public List<Pelicula>? PeliculasGenero { get; set; }
    }
}
