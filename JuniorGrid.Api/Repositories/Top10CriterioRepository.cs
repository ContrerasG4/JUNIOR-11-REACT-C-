using JuniorGrid.Api.Data;
using JuniorGrid.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace JuniorGrid.Api.Repositories
{

    public class Top10CriterioRepository : ITop10CriterioRepository
    {
        private readonly AppDbContext _context;

        public Top10CriterioRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Top10Criterio>> ObtenerTodosAsync()
        {
            return await _context.Top10Criterios
                .AsNoTracking()
                .ToListAsync();
        }
    }
}