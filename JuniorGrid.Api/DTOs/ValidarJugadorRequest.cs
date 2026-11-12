using System.ComponentModel.DataAnnotations;

namespace JuniorGrid.Api.DTOs
{
    public class ValidarJugadorRequestDto
    {
        [Required(ErrorMessage = "El nombre del jugador es obligatorio.")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "El nombre del jugador debe tener entre 1 y 100 caracteres.")]
        public string NombreJugador { get; set; } = string.Empty;

        [Required(ErrorMessage = "El ID del criterio de fila es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "El ID del criterio de fila debe ser un valor válido mayor a 0.")]
        public int CriterioFilaId { get; set; }

        [Required(ErrorMessage = "El ID del criterio de columna es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "El ID del criterio de columna debe ser un valor válido mayor a 0.")]
        public int CriterioColumnaId { get; set; }
    }
}