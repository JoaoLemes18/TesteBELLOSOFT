namespace API.Dtos
{
    public class AuthResultDtos
    {
        public bool Success { get; set; }
        public string? Token { get; set; }
        public string Message { get; set; } = string.Empty;
        public string? Nome { get; set; }
        public string? Email { get; set; }
    }
}
