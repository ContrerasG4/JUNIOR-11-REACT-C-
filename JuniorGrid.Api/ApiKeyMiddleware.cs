public class ApiKeyMiddleware
{
    private readonly RequestDelegate _next;
    private readonly string _apiKey;
    private const string ApiKeyHeaderName = "X-API-KEY";

    public ApiKeyMiddleware(
        RequestDelegate next,
        IConfiguration configuration)
    {
        _next = next;

        _apiKey = configuration["ApiKey"]
            ?? throw new InvalidOperationException(
                "No se ha configurado la API Key.");
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (HttpMethods.IsPost(context.Request.Method) &&
            string.Equals(
                context.Request.Path.Value?.TrimEnd('/'),
                "/api/auth/sesion",
                StringComparison.OrdinalIgnoreCase))
        {
            if (!context.Request.Headers.TryGetValue(
                    ApiKeyHeaderName, out var extractedApiKey)
                || !string.Equals(
                    _apiKey,
                    extractedApiKey.ToString(),
                    StringComparison.Ordinal))
            {
                context.Response.StatusCode =
                    StatusCodes.Status401Unauthorized;

                await context.Response.WriteAsJsonAsync(new
                {
                    mensaje = "API Key inválida o ausente."
                });

                return;
            }
        }

        await _next(context);
    }
}