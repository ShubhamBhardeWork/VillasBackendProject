namespace Villas.API.DTOs
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public int StatusCode { get; set; }
        public required string Message { get; set; }
        public required string TraceId { get; set; }
        public T? Data { get; set; }
        public List<string>? Errors { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;


        private static ApiResponse<T> Create(
            bool success,
            int statusCode,
            string message,
            string traceId,
            T? data = default,
            List<string>? errors = null)
        {
            return new ApiResponse<T>
            {
                Success = success,
                StatusCode = statusCode,
                Message = message,
                TraceId = traceId,
                Data = data,
                Errors = errors
            };
        }

        public static ApiResponse<T> Ok(T data, string message, string traceId)
        {

            return Create(true, StatusCodes.Status200OK, message, traceId, data);
        }

        public static ApiResponse<T> Created(T data, string message, string traceId)
        {
            return Create(true, StatusCodes.Status201Created, message, traceId, data);
        }

        public static ApiResponse<T> BadRequest(string message, string traceId, List<string>? errors = null)
        {
            return Create(false, StatusCodes.Status400BadRequest, message, traceId, default, errors);
        }

        public static ApiResponse<T> NotFound(string message, string traceId)
        {
            return Create(false, StatusCodes.Status404NotFound, message, traceId);
        }

        public static ApiResponse<T> Conflict(string message, string traceId, List<string>? errors = null)
        {
            return Create(false, StatusCodes.Status409Conflict, message, traceId, default, errors);
        }

        public static ApiResponse<T> InternalServerError(string message, string traceId, List<string>? errors = null)
        {
            return Create(false, StatusCodes.Status500InternalServerError, message, traceId, default, errors);
        }
    }
}
