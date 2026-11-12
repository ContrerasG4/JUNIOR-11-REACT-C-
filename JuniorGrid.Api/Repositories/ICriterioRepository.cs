using JuniorGrid.Api.Models;

namespace JuniorGrid.Api.Repositories
{
    public interface ICriterioRepository
    {
        Task<List<Criterio>> ObtenerTodosAsync();
        Task<Criterio?> ObtenerPorIdAsync(int id);
    }
}
