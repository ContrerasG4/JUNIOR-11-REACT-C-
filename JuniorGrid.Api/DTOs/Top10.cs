using System.ComponentModel.DataAnnotations;

namespace JuniorGrid.Api.DTOs
{
    public class PosicionTop10Dto
    {
        [Required(ErrorMessage = "La posición es obligatoria.")]
        [Range(1, 10, ErrorMessage = "La posición debe estar entre 1 y 10.")]
        public int Posicion { get; set; }

        [Required(ErrorMessage = "El nombre del jugador es obligatorio.")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "El nombre debe tener entre 1 y 100 caracteres.")]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "La nacionalidad es obligatoria.")]
        [StringLength(50, ErrorMessage = "La nacionalidad no puede superar los 50 caracteres.")]
        public string Nacionalidad { get; set; } = string.Empty;

        [Required(ErrorMessage = "La década es obligatoria.")]
        [StringLength(20, ErrorMessage = "La década no puede superar los 20 caracteres.")]
        public string Decada { get; set; } = string.Empty;
    }

    public class Top10RespuestaDto
    {
        [Required(ErrorMessage = "El título del reto es obligatorio.")]
        [StringLength(150, ErrorMessage = "El título del reto no puede superar los 150 caracteres.")]
        public string TituloReto { get; set; } = string.Empty;

        [Required(ErrorMessage = "La lista de posiciones es obligatoria.")]
        public List<PosicionTop10Dto> Posiciones { get; set; } = new();
    }
}