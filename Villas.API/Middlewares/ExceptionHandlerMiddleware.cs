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

                    var errorResponse = new ApiResponse<object>
                    {
                        Success = false,
                        StatusCode = StatusCodes.Status409Conflict,
                        Message = "Villa name already exists.",
                        Errors = new List<string> { "Duplicate villa name is not allowed." },
                        TraceId = context.TraceIdentifier
                    };
                    await context.Response.WriteAsJsonAsync(errorResponse);
                }
            }

            catch (Exception ex)
            {
                if (!context.Response.HasStarted)
                {
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    context.Response.ContentType = "application/json";

                    var errorResponse = new ApiResponse<object>
                    {
                        Success = false,
                        StatusCode = StatusCodes.Status500InternalServerError,
                        Message = "Something went wrong.",
                        Errors = new List<string> { "Internal Server Error." },
                        TraceId = context.TraceIdentifier
                    };
                    await context.Response.WriteAsJsonAsync(errorResponse);
                }
            }
        }
    }
}
