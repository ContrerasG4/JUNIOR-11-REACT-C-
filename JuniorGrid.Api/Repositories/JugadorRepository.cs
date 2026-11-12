using JuniorGrid.Api.Data;
using JuniorGrid.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace JuniorGrid.Api.Repositories
{
    public class JugadorRepository : IJugadorRepository
    {
        private readonly AppDbContext _context;

        public JugadorRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Jugador>> ObtenerTodosAsync()
        {
            return await _context.Jugadores.AsNoTracking().ToListAsync();
        }

        public async Task<Jugador?> ObtenerPorNombreAsync(string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre)) return null;

            string searchTerm = nombre.Trim().ToLower();

            var jugador = await _context.Jugadores
                .AsNoTracking()
                .FirstOrDefaultAsync(j => j.Nombre.ToLower() == searchTerm);

            if (jugador == null)
            {
                jugador = await _context.Jugadores
                    .AsNoTracking()
                    .FirstOrDefaultAsync(j => j.Nombre.ToLower().Contains(searchTerm));
            }

            return jugador;
        }
    }
}