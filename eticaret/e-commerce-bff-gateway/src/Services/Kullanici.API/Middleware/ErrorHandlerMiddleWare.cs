using System.Net;
using System.Text.Json;

namespace Kullanici.API.Middleware
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlerMiddleware> _logger;

        public ErrorHandlerMiddleware(RequestDelegate next, ILogger<ErrorHandlerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                
                await _next(context);
            }
            catch (Exception error)
            {
                var response = context.Response;
                response.ContentType = "application/json";

                
                switch (error)
                {
                    
                    case Exception e when e.Message.Contains("already in use") || e.Message.Contains("not found") || e.Message.Contains("Wrong password"):
                        response.StatusCode = (int)HttpStatusCode.BadRequest; // 400 Bad Request
                        break;
                    default:
                        
                        response.StatusCode = (int)HttpStatusCode.InternalServerError; // 500 Internal Server Error
                        break;
                }

                _logger.LogError(error, "Bir hata yakalandi: {Message}", error.Message);

                var result = JsonSerializer.Serialize(new { message = error?.Message });
                await response.WriteAsync(result);
            }
        }
    }
}