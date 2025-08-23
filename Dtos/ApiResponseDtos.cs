namespace API.Dtos
{
    public class ApiResponseDtos<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; }

        private ApiResponseDtos(bool success, string message, T? data = default)
        {
            Success = success;
            Message = message;
            Data = data;
        }

        public static ApiResponseDtos<T> Ok(T? data, string message = "Operação concluída com sucesso.")
            => new ApiResponseDtos<T>(true, message, data);

        public static ApiResponseDtos<T> Fail(string message)
            => new ApiResponseDtos<T>(false, message);
    }
}
