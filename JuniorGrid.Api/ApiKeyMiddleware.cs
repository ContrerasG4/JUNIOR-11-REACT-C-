public class ApiKeyMiddleware
{
	private readonly RequestDelegate _next;
	private const string ApiKeyHeaderName = "X-API-KEY";

	public ApiKeyMiddleware(RequestDelegate next)
	{
		_next = next;
	}

	public async Task InvokeAsync(HttpContext context)
	{
		// Si no viene la cabecera, rechazamos
		if (!context.Request.Headers.TryGetValue(ApiKeyHeaderName, out var extractedApiKey))
		{
			context.Response.StatusCode = 401; 
			await context.Response.WriteAsync("Falta la API Key.");
			return;
		}

		// Compara con la clave del appsettings
		var appSettings = context.RequestServices.GetRequiredService<IConfiguration>();
		var apiKey = appSettings.GetValue<string>("ApiKey");

		if (!apiKey.Equals(extractedApiKey))
		{
			context.Response.StatusCode = 401;
			await context.Response.WriteAsync("API Key inválida.");
			return;
		}

		await _next(context);
	}
}