using JuniorGrid.Api.DTOs;
using JuniorGrid.Api.Models;
using JuniorGrid.Api.Repositories;

namespace JuniorGrid.Api.Services
{
    public class GameService : IGameService
    {
        private readonly IJugadorRepository _jugadorRepo;
        private readonly ICriterioRepository _criterioRepo;

        public GameService(IJugadorRepository jugadorRepo, ICriterioRepository criterioRepo)
        {
            _jugadorRepo = jugadorRepo;
            _criterioRepo = criterioRepo;
        }

        public async Task<TableroResponseDto?> GenerarTableroAleatorioAsync()
        {
            var criterios = await _criterioRepo.ObtenerTodosAsync();
            var jugadores = await _jugadorRepo.ObtenerTodosAsync();

            if (criterios == null || !criterios.Any() || jugadores == null || !jugadores.Any())
                return null;

            Random rand = new Random();
            bool tableroValido = false;
            List<Criterio> filas = new();
            List<Criterio> columnas = new();

            int intentos = 0;
            while (!tableroValido && intentos < 100)
            {
                intentos++;
                var mezcla = criterios.OrderBy(x => rand.Next()).ToList();
                if (mezcla.Count < 6) break; 

                filas = mezcla.Take(3).ToList();
                columnas = mezcla.Skip(3).Take(3).ToList();

                tableroValido = ValidarMatriz(filas, columnas, jugadores);
            }

            if (!tableroValido) return null;

            return new TableroResponseDto
            {
                Filas = filas.Select(f => new CriterioDto { Id = f.CriterioId, Etiqueta = f.Etiqueta }).ToList(),
                Columnas = columnas.Select(c => new CriterioDto { Id = c.CriterioId, Etiqueta = c.Etiqueta }).ToList()
            };
        }

        public async Task<ValidacionRespuestaDto> ValidarCasillaAsync(ValidarJugadorRequestDto request)
        {
            var jugador = await _jugadorRepo.ObtenerPorNombreAsync(request.NombreJugador);
            var fila = await _criterioRepo.ObtenerPorIdAsync(request.CriterioFilaId);
            var col = await _criterioRepo.ObtenerPorIdAsync(request.CriterioColumnaId);

            if (jugador == null || fila == null || col == null)
                return new ValidacionRespuestaDto
                {
                    EsValido = false,
                    Mensaje = jugador == null ? $"Jugador '{request.NombreJugador}' no encontrado." : "Criterios no encontrados."
                };

            bool cumpleFila = EvaluarCriterio(jugador, fila);
            bool cumpleCol = EvaluarCriterio(jugador, col);
            bool esValido = cumpleFila && cumpleCol;

            string mensajeResultado = esValido
                ? "¡Acierto total!"
                : $"{jugador.Nombre} no cumple con ambas condiciones.";

            return new ValidacionRespuestaDto
            {
                EsValido = esValido,
                Jugador = jugador.Nombre,
                Mensaje = mensajeResultado
            };
        }

        private bool ValidarMatriz(List<Criterio> filas, List<Criterio> cols, List<Jugador> jugadores)
        {
            foreach (var f in filas)
            {
                foreach (var c in cols)
                {
                    bool existe = jugadores.Any(j => EvaluarCriterio(j, f) && EvaluarCriterio(j, c));
                    if (!existe) return false;
                }
            }
            return true;
        }


        private bool EvaluarCriterio(Jugador j, Criterio c)
        {
            bool esNumeroValido = int.TryParse(c.ValorReferencia, out int valorRef);

            return c.Tipo switch
            {
                "GOLES" => esNumeroValido && j.Goles >= valorRef,
                "PARTIDOS" => esNumeroValido && j.Partidos >= valorRef,
                "TITULOS" => esNumeroValido && j.Titulos >= valorRef,
                "CICLOS" => esNumeroValido && j.Ciclos >= valorRef,
                "NACIONALIDAD" => c.Operador == "=="
                    ? j.Nacionalidad.Equals(c.ValorReferencia, StringComparison.OrdinalIgnoreCase)
                    : !j.Nacionalidad.Equals(c.ValorReferencia, StringComparison.OrdinalIgnoreCase),
                "DECADA" => j.Decada.Contains(c.ValorReferencia, StringComparison.OrdinalIgnoreCase),
                _ => false
            };
        }
     
    }
}