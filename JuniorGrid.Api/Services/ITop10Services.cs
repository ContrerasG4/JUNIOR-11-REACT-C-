using JuniorGrid.Api.DTOs;
namespace JuniorGrid.Api.Services
{
    public interface ITop10CriterioService
    {
        Task<Top10RespuestaDto?> GenerarRetoAleatorioAsync();
    }
}