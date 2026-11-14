using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IConfiguration _config;
    public AuthController(IConfiguration config) { _config = config; }

    [HttpPost("sesion")]
    public IActionResult CrearSesion()
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var jwtSecret = _config["JwtSettings:SecretKey"] ?? "ClavePorDefectoParaDesarrollo1234567890";
        var key = Encoding.ASCII.GetBytes(jwtSecret);

        // Creamos el token (dura 2 horas, por ejemplo)
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim("SessionId", Guid.NewGuid().ToString()),
                new Claim("IP", HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown")
            }),
            Expires = DateTime.UtcNow.AddHours(2),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return Ok(new { token = tokenHandler.WriteToken(token) });
    }
}