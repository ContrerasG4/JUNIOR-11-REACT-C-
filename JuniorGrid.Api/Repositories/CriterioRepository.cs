using JuniorGrid.Api.Data;
using JuniorGrid.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace JuniorGrid.Api.Repositories
{
    public class CriterioRepository : ICriterioRepository
    {
        private readonly AppDbContext _context;

        public CriterioRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Criterio>> ObtenerTodosAsync()
        {
            return await _context.Criterios.ToListAsync();
        }

        public async Task<Criterio?> ObtenerPorIdAsync(int id)
        {
            return await _context.Criterios.FindAsync(id);
        }
    }
}
