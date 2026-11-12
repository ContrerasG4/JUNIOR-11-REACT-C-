using JuniorGrid.Api.Models;
namespace JuniorGrid.Api.Repositories
{
    public interface ITop10CriterioRepository
    {
        Task<IEnumerable<Top10Criterio>> ObtenerTodosAsync();
    }
}