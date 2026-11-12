using JuniorGrid.Api.DTOs;
using JuniorGrid.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace JuniorGrid.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GameGridController : ControllerBase
    {
        private readonly IGameService _gameService;
        private readonly ILogger<GameGridController> _logger; 

        public GameGridController(IGameService gameService, ILogger<GameGridController> logger)
        {
            _gameService = gameService;
            _logger = logger;
        }

        [HttpGet("nuevo-tablero")]
        public async Task<IActionResult> ObtenerNuevoTablero()
        {
            try
            {
                var tablero = await _gameService.GenerarTableroAleatorioAsync();

                if (tablero == null)
                {
                    _logger.LogWarning("Intento de generar tablero fallido: No se encontraron datos válidos.");
                    return NotFound(new { mensaje = "No hay suficientes criterios o jugadores para armar el tablero." });
                }

                return Ok(tablero);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error crítico al generar el tablero aleatorio.");
                return StatusCode(500, new { mensaje = "Ocurrió un error interno en el servidor al generar el tablero." });
            }
        }

        [HttpPost("validar")]
        public async Task<IActionResult> ValidarJugador([FromBody] ValidarJugadorRequestDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (request == null || string.IsNullOrWhiteSpace(request.NombreJugador))
            {
                return BadRequest(new { mensaje = "El objeto de la petición o el nombre del jugador no pueden estar vacíos." });
            }

            try
            {
                var resultado = await _gameService.ValidarCasillaAsync(request);

                if (resultado == null)
                {
                    return NotFound(new { mensaje = "No se pudo procesar la validación para el jugador indicado." });
                }

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al validar el jugador: {NombreJugador}", request.NombreJugador);
                return StatusCode(500, new { mensaje = "Ocurrió un error interno al validar la jugada." });
            }
        }
    }
}