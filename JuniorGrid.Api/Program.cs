using System.Threading.RateLimiting;
using JuniorGrid.Api.Data;
using JuniorGrid.Api.Repositories;
using JuniorGrid.Api.Services;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Kestrel: sin header Server y body máximo de 10 KB
builder.WebHost.ConfigureKestrel(o =>
{
    o.AddServerHeader = false;
    o.Limits.MaxRequestBodySize = 10 * 1024;
});

var connectionString = builder.Configuration.GetConnectionString("Conexion");
Console.WriteLine($"--> CONEXIÓN EN USO: {connectionString}");


builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Conexion")));

// Repositorios y servicios
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IJugadorRepository, JugadorRepository>();
builder.Services.AddScoped<ICriterioRepository, CriterioRepository>();
builder.Services.AddScoped<ITop10CriterioRepository, Top10CriterioRepository>();

builder.Services.AddScoped<IGameService, GameService>();
builder.Services.AddScoped<ITop10CriterioService, Top10CriterioService>();

// Solo si se despliega detrás de un proxy (Nginx, Azure, etc.), se activa con "BehindProxy": true
var behindProxy = builder.Configuration.GetValue<bool>("BehindProxy");
if (behindProxy)
{
    builder.Services.Configure<ForwardedHeadersOptions>(o =>
    {
        o.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
        // TODO: agregar la IP del proxy en KnownProxies cuando se defina
    });
}

// Rate limit por IP: 60 peticiones por minuto
builder.Services.AddRateLimiter(options =>
{
    options.OnRejected = async (context, cancellationToken) =>
    {
        context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
        context.HttpContext.Response.ContentType = "application/json";
        await context.HttpContext.Response.WriteAsync(
            "{\"mensaje\": \"Has realizado demasiadas peticiones. Por favor, espera un momento.\"}",
            cancellationToken
        );
    };

    options.AddPolicy("IpRateLimit", httpContext =>
    {
        // Si es una petición preflight (OPTIONS), no la contamos en el límite
        if (HttpMethods.IsOptions(httpContext.Request.Method))
        {
            return RateLimitPartition.GetNoLimiter("preflight");
        }
        var clientIp = httpContext.Connection.RemoteIpAddress?.ToString() ?? "anonimo";

        return RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: clientIp,
            factory: _ => new FixedWindowRateLimiterOptions
            {
                AutoReplenishment = true,
                PermitLimit = 60,
                Window = TimeSpan.FromMinutes(1),
                QueueLimit = 2,  // deja 2 en cola, el resto va directo a 429
                QueueProcessingOrder = QueueProcessingOrder.OldestFirst
            }
        );
    });
});

// CORS: solo los orígenes del front
var allowedOrigins = builder.Configuration.GetSection("AllowedOrigins").Get<string[]>();

if (allowedOrigins is null || allowedOrigins.Length == 0)
{
    if (builder.Environment.IsDevelopment())
    {
        allowedOrigins = new[] { "http://localhost:5173", "http://localhost:3000" };
    }
    else
    {
        // En producción no dejamos arrancar sin orígenes configurados
        throw new InvalidOperationException("Falta configurar 'AllowedOrigins' para producción.");
    }
}

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
        policy.WithOrigins(allowedOrigins)
              .AllowAnyHeader() 
              .AllowAnyMethod() 
    );
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Pipeline (el orden importa)

// Antes del rate limiter, para que lea la IP real del cliente
if (behindProxy)
{
    app.UseForwardedHeaders();
}

app.UseCors("AllowFrontend");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    //app.UseHttpsRedirection();

    app.UseExceptionHandler(errorApp => errorApp.Run(async ctx =>
    {
        var ex = ctx.Features.Get<Microsoft.AspNetCore.Diagnostics.IExceptionHandlerFeature>()?.Error;

        // Si el error es del cliente (body muy grande, request mal formado) se respeta su código
        var status = ex is BadHttpRequestException bad ? bad.StatusCode : 500;

        var mensaje = status switch
        {
            413 => "La solicitud es demasiado grande.",
            < 500 => "Solicitud inválida.",
            _ => "Ocurrió un error inesperado."
        };

        ctx.Response.StatusCode = status;
        ctx.Response.ContentType = "application/json";
        await ctx.Response.WriteAsync($"{{\"mensaje\":\"{mensaje}\"}}");
    }));

    app.UseHsts();
}

// Aplicación automática de migraciones de base de datos al arrancar
//using (var scope = app.Services.CreateScope())
//{
//    var services = scope.ServiceProvider;
//    try
//    {
//        var dbContext = services.GetRequiredService<AppDbContext>();
//        await dbContext.Database.MigrateAsync();
//    }
//    catch (Exception ex)
//    {
//        var logger = services.GetRequiredService<ILogger<Program>>();
//        logger.LogError(ex, "Error al aplicar migraciones automáticas en la base de datos.");
//    }
//}


app.UseRateLimiter();

app.UseAuthorization();

app.MapControllers().RequireRateLimiting("IpRateLimit");

app.Run();