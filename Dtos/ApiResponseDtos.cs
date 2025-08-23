namespace API.Dtos
{

    /// <summary>
    /// Envelope padrão para respostas de API, padronizando sucesso/erro e mensagem.
    /// </summary>
    /// <typeparam name="T">Tipo do objeto retornado em <see cref="Data"/>.</typeparam>
    public class ApiResponseDtos<T>
    {

        /// <summary>Indica se a operação foi bem-sucedida.</summary>
        public bool Success { get; set; }

        /// <summary>Mensagem amigável para o cliente.</summary>
        public string Message { get; set; } = string.Empty;

        /// <summary>Dados retornados (quando houver).</summary>
        public T? Data { get; set; }

        private ApiResponseDtos(bool success, string message, T? data = default)
        {
            Success = success;
            Message = message;
            Data = data;
        }


        /// <summary>Cria uma resposta de sucesso.</summary>
        public static ApiResponseDtos<T> Ok(T? data, string message = "Operação concluída com sucesso.")
            => new ApiResponseDtos<T>(true, message, data);


        /// <summary>Cria uma resposta de erro.</summary>
        public static ApiResponseDtos<T> Fail(string message)
            => new ApiResponseDtos<T>(false, message);
    }
}
