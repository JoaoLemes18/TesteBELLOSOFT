using System.ComponentModel.DataAnnotations;

namespace API.Dtos
{
    /// <summary>
    /// Dados para cadastro de um novo usuário.
    /// </summary>
    /// <remarks>Usado em <c>POST /api/auth/register</c>.</remarks> 
    public class RegisterRequest
    {
        [Required(ErrorMessage = "O nome é obrigatório.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "O nome deve ter entre 2 e 100 caracteres.")]
        /// <summary>Nome do usuário.</summary>

        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "O email é obrigatório.")]
        [EmailAddress(ErrorMessage = "Formato de email inválido.")]
        /// <summary>E-mail do usuário.</summary>
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "A senha é obrigatória.")]
        [MinLength(6, ErrorMessage = "A senha deve ter no mínimo 6 caracteres.")]
        /// <summary>Senha em texto simples (apenas para o exercício).</summary>

        public string Senha { get; set; } = string.Empty;
    }

    /// <summary>
    /// Credenciais de login.
    /// </summary>
    /// <remarks>Usado em <c>POST /api/auth/login</c>.</remarks>
    public class LoginRequest
    {
        [Required(ErrorMessage = "O email é obrigatório.")]
        [EmailAddress(ErrorMessage = "Formato de email inválido.")]
        /// <summary>E-mail do usuário.</summary>

        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "A senha é obrigatória.")]
        /// <summary>Senha do usuário.</summary>

        public string Senha { get; set; } = string.Empty;
    }

    /// <summary>
    /// Resposta do login com o token JWT e dados básicos do usuário.
    /// </summary>
    public class LoginResponse
    {       
        
        /// <summary>Token JWT para autenticação nas próximas requisições.</summary>
        public string Token { get; set; } = string.Empty;

        /// <summary>Nome do usuário autenticado.</summary>
        public string Nome { get; set; } = string.Empty;

        /// <summary>E-mail do usuário autenticado.</summary>

        public string Email { get; set; } = string.Empty;
    }
}
