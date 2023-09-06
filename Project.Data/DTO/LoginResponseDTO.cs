using Project.Data.Models;

namespace Project.Data.DTO
{
    public class LoginResponseDTO
    {
        public LocalUser User { get; set; }

        public string Token { get; set; }
    }
}
