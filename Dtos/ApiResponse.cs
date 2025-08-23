namespace API.Dtos
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; }

        private ApiResponse(bool success, string message, T? data = default)
        {
            Success = success;
            Message = message;
            Data = data;
        }

        public static ApiResponse<T> Ok(T? data, string message = "Operação concluída com sucesso.")
            => new ApiResponse<T>(true, message, data);

        public static ApiResponse<T> Fail(string message)
            => new ApiResponse<T>(false, message);
    }
}
