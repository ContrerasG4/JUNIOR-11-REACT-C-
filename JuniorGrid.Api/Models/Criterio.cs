using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JuniorGrid.Api.Models
{
    [Table("Criterios")]
    public class Criterio
    {
        [Key]
        public int CriterioId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Etiqueta { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string Tipo { get; set; } = string.Empty; 

        [Required]
        [MaxLength(50)]
        public string ValorReferencia { get; set; } = string.Empty;

        [Required]
        [MaxLength(10)]
        public string Operador { get; set; } = string.Empty; 
    }
}