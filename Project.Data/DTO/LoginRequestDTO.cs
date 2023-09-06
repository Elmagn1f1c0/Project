
using System.ComponentModel.DataAnnotations.Schema;

namespace Project.Data.DTO
{
    public class LoginRequestDTO
    {
       
        public string UsernameOrEmail { get; set; }
        
        public string Password { get; set;}
    }
}
