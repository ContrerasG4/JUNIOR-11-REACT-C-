namespace JuniorGrid.Api.Models
{
    public class Top10Criterio
    {
        public int CriterioId { get; set; }
        public string Etiqueta { get; set; } = string.Empty;
        public string Tipo { get; set; } = string.Empty;
        public string? ValorReferencia { get; set; }
        public string? Operador { get; set; }
        public string? Dificultad { get; set; }
    }
}