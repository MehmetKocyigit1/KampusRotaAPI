namespace KampusRota.Services
{
    public sealed record ServiceResult<T>(bool Success, T? Value, string Message, int StatusCode)
    {
        public static ServiceResult<T> Ok(T value) => new(true, value, string.Empty, 200);
        public static ServiceResult<T> Created(T value) => new(true, value, string.Empty, 201);
        public static ServiceResult<T> BadRequest(string message) => new(false, default, message, 400);
        public static ServiceResult<T> NotFound(string message) => new(false, default, message, 404);
        public static ServiceResult<T> Forbidden(string message) => new(false, default, message, 403);
    }
}
