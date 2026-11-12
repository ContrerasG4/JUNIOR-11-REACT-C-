namespace JuniorGrid.Api.DTOs
{
    public class ValidacionRespuestaDto
    {
        public bool EsValido { get; set; }
        public string Jugador { get; set; } = string.Empty;
        public string Mensaje { get; set; } = string.Empty;
    }
}