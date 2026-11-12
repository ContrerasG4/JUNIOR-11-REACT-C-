using JuniorGrid.Api.DTOs;

namespace JuniorGrid.Api.Services
{
    public interface IGameService
    {
        Task<TableroResponseDto?> GenerarTableroAleatorioAsync();
        Task<ValidacionRespuestaDto> ValidarCasillaAsync(ValidarJugadorRequestDto request);
    }
}
