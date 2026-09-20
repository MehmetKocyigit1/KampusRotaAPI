namespace KampusRota.Application.Common
{
    public class ServiceResult<T>
    {
        public bool Success { get; set; }
        public string? ErrorMessage { get; set; }
        public string? Message => ErrorMessage;
        public int StatusCode { get; set; }
        public T? Value { get; set; }

        public static ServiceResult<T> Ok(T value, int statusCode = 200)
        {
            return new ServiceResult<T> { Success = true, Value = value, StatusCode = statusCode };
        }

        public static ServiceResult<T> Created(T value)
        {
            return new ServiceResult<T> { Success = true, Value = value, StatusCode = 201 };
        }

        public static ServiceResult<T> Fail(string message, int statusCode = 400)
        {
            return new ServiceResult<T> { Success = false, ErrorMessage = message, StatusCode = statusCode };
        }

        public static ServiceResult<T> BadRequest(string message) => Fail(message, 400);
        public static ServiceResult<T> NotFound(string message) => Fail(message, 404);
        public static ServiceResult<T> Forbidden(string message) => Fail(message, 403);
    }
}
