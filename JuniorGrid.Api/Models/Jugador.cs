using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JuniorGrid.Api.Models
{
    [Table("Jugadores")]
    public class Jugador
    {
        [Key]
        public int JugadorId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string Nacionalidad { get; set; } = string.Empty;

        public int Partidos { get; set; } = 0;

        public int Goles { get; set; } = 0;

        public int Asistencias { get; set; } = 0;

        public int Titulos { get; set; } = 0;

        [Required]
        [MaxLength(20)]
        public string Decada { get; set; } = string.Empty;

        public int Ciclos { get; set; } = 1;

        [MaxLength(255)]
        public string? FotoUrl { get; set; }
    }
}