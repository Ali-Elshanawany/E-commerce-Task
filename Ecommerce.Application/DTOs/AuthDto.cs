using System.Text.Json.Serialization;

namespace Ecommerce.Application.DTOs
{
    public class AuthDto
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }

        public string? RefreshToken { get; set; }
        public bool IsAuthenticated { get; set; }
        public DateTime LastLogin { get; set; }
        public DateTime RefreshTokenExpiration { get; set; }

        //public DateTime ExpiresOn { get; set; }
    }
}
