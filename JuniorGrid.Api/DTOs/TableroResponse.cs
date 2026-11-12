namespace JuniorGrid.Api.DTOs
{
    public class TableroResponseDto
    {
        public List<CriterioDto> Filas { get; set; } = new();
        public List<CriterioDto> Columnas { get; set; } = new();
    }
}
