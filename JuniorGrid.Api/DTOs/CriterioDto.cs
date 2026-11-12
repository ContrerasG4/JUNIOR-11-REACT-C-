using System.ComponentModel.DataAnnotations;

namespace JuniorGrid.Api.DTOs
{
    public class CriterioDto
    {
        [Required(ErrorMessage = "El ID del criterio es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "El ID del criterio debe ser mayor a 0.")]
        public int Id { get; set; }

        [Required(ErrorMessage = "La etiqueta es obligatoria.")]
        [StringLength(150, MinimumLength = 1, ErrorMessage = "La etiqueta debe tener entre 1 y 150 caracteres.")]
        public string Etiqueta { get; set; } = string.Empty;

        [Required(ErrorMessage = "El tipo de criterio es obligatorio.")]
        [StringLength(50, ErrorMessage = "El tipo no puede superar los 50 caracteres.")]
        public string Tipo { get; set; } = string.Empty;

        [Required(ErrorMessage = "El valor de referencia es obligatorio.")]
        [StringLength(100, ErrorMessage = "El valor de referencia no puede superar los 100 caracteres.")]
        public string ValorReferencia { get; set; } = string.Empty;

        [Required(ErrorMessage = "El operador es obligatorio.")]
        [StringLength(30, ErrorMessage = "El operador no puede superar los 30 caracteres.")]
        public string Operador { get; set; } = string.Empty;
      
        public int Dificultad { get; set; }
    }
}