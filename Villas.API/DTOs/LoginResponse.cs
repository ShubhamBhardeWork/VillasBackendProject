
namespace Villas.API.DTOs
{
    public class LoginResponse
    {
        public string? AccessToken { get; set; } 
        public UserResponse? User { get; set; }
    }
}
