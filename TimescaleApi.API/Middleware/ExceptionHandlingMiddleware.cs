using System.Net;
using System.Text.Json;

namespace TimescaleApi.API.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred: ", ex.Message);
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;

               await context.Response.WriteAsJsonAsync($"Ошибка сервера: {ex.Message}");
            }
        }
    }
}
