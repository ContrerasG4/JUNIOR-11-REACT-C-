using JuniorGrid.Api.DTOs;
using JuniorGrid.Api.Repositories;
using JuniorGrid.Api.Models;

namespace JuniorGrid.Api.Services
{

    public class Top10CriterioService : ITop10CriterioService
    {
        private readonly ITop10CriterioRepository _top10Repo;
        private readonly IJugadorRepository _jugadorRepo;

        public Top10CriterioService(ITop10CriterioRepository top10Repo, IJugadorRepository jugadorRepo)
        {
            _top10Repo = top10Repo;
            _jugadorRepo = jugadorRepo;
        }

        public async Task<Top10RespuestaDto?> GenerarRetoAleatorioAsync()
        {
            var criterios = await _top10Repo.ObtenerTodosAsync();
            var jugadores = await _jugadorRepo.ObtenerTodosAsync();

            if (!criterios.Any() || !jugadores.Any()) return null;

            // Seleccionar un criterio/reto al azar de la tabla Top10Criterios
            var rand = new Random();
            var criterioElegido = criterios.ElementAt(rand.Next(criterios.Count()));

            var query = jugadores.AsQueryable();

            // Aplicar filtros según el Tipo y ValorReferencia
            if (!string.IsNullOrEmpty(criterioElegido.Tipo) && !string.IsNullOrEmpty(criterioElegido.ValorReferencia))
            {
                if (criterioElegido.Tipo.Equals("DECADA", StringComparison.OrdinalIgnoreCase))
                {
                    query = query.Where(j => j.Decada.Contains(criterioElegido.ValorReferencia));
                }
                else if (criterioElegido.Tipo.Equals("NACIONALIDAD", StringComparison.OrdinalIgnoreCase))
                {
                    if (criterioElegido.Operador == "!=")
                        query = query.Where(j => !j.Nacionalidad.Equals(criterioElegido.ValorReferencia, StringComparison.OrdinalIgnoreCase));
                    else
                        query = query.Where(j => j.Nacionalidad.Equals(criterioElegido.ValorReferencia, StringComparison.OrdinalIgnoreCase));
                }
            }

            // Ordenamiento dinámico según el tipo de métrica o el criterio
            var top10Jugadores = criterioElegido.Tipo.ToUpper() switch
            {
                "PARTIDOS" => query.OrderByDescending(j => j.Partidos).Take(10).ToList(),
                "ASISTENCIAS" => query.OrderByDescending(j => j.Asistencias).Take(10).ToList(),
                "TITULOS" => query.OrderByDescending(j => j.Titulos).Take(10).ToList(),
                "CICLOS" => query.OrderByDescending(j => j.Ciclos).Take(10).ToList(),
                _ => query.OrderByDescending(j => j.Goles).Take(10).ToList() // GOLES y DECADA por defecto ordenan por goles
            };

            // Respaldo de seguridad si el filtro es muy restrictivo
            if (top10Jugadores.Count < 3)
            {
                top10Jugadores = jugadores.OrderByDescending(j => j.Goles).Take(10).ToList();
            }

            return new Top10RespuestaDto
            {
                TituloReto = criterioElegido.Etiqueta,
                Posiciones = top10Jugadores.Select((j, index) => new PosicionTop10Dto
                {
                    Posicion = index + 1,
                    Nombre = j.Nombre,
                    Nacionalidad = j.Nacionalidad,
                    Decada = j.Decada
                }).ToList()
            };
        }
    }
}