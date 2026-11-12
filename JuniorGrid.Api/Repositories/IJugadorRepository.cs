using JuniorGrid.Api.Models;

namespace JuniorGrid.Api.Repositories
{
    public interface IJugadorRepository
    {
        Task<List<Jugador>> ObtenerTodosAsync();
        Task<Jugador?> ObtenerPorNombreAsync(string nombre);
    }
}
