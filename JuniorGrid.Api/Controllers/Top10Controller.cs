using JuniorGrid.Api.DTOs;
using JuniorGrid.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
namespace JuniorGrid.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/top10")]
    public class Top10Controller : ControllerBase
    {
        private readonly ITop10CriterioService _top10Service;
        private readonly ILogger<Top10Controller> _logger; 

        public Top10Controller(ITop10CriterioService top10Service, ILogger<Top10Controller> logger)
        {
            _top10Service = top10Service;
            _logger = logger;
        }

        [HttpGet("reto")]
        public async Task<IActionResult> ObtenerRetoTop10()
        {
            try
            {
                var reto = await _top10Service.GenerarRetoAleatorioAsync();

                if (reto == null)
                {
                    _logger.LogWarning("Intento de generar reto Top 10 fallido: No hay criterios o jugadores disponibles.");
                    return NotFound(new { mensaje = "No hay suficientes criterios o jugadores disponibles para generar el reto." });
                }

                return Ok(reto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error crítico al generar el reto Top 10 aleatorio.");
                return StatusCode(500, new { mensaje = "Ocurrió un error interno en el servidor al procesar el reto." });
            }
        }
    }
}