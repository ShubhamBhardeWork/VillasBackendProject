using Microsoft.EntityFrameworkCore;
using Villas.API.DTOs;

namespace Villas.API.Middlewares
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var traceId = context.TraceIdentifier;

            try
            {
                await _next(context);
            }

            catch (DbUpdateException)
            {
                if (!context.Response.HasStarted)
                {
                    context.Response.StatusCode = StatusCodes.Status409Conflict;
                    context.Response.ContentType = "application/json";

                    var errorResponse = ApiResponse<object>.Conflict(
                        "Villa name already exists.", 
                        traceId, 
                        new List<string> { "Duplicate villa name is not allowed." }
                    );
                    await context.Response.WriteAsJsonAsync(errorResponse);
                }
            }

            catch (Exception ex)
            {
                if (!context.Response.HasStarted)
                {
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    context.Response.ContentType = "application/json";

                    var errorResponse = ApiResponse<object>.InternalServerError(
                        "Something went wrong", 
                        traceId, 
                        new List<string> { "Internal Server Error." }
                    );
                    await context.Response.WriteAsJsonAsync(errorResponse);
                }
            }
        }
    }
}
