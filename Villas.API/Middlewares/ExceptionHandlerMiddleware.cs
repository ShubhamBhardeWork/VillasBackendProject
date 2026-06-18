using Microsoft.EntityFrameworkCore;

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

                    var errorResponse = new
                    {
                        Success = false,
                        Message = "Villa name already exists."
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

                    var errorResponse = new
                    {
                        Success = false,
                        Message = "Something Went Wrong."
                    };
                    await context.Response.WriteAsJsonAsync(errorResponse);
                }
            }
        }
    }
}
