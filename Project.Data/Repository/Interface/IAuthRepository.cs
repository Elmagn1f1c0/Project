using Project.Data.DTO;
using Project.Data.Models;

namespace Project.Data.Repository.Interface
{
    public interface IAuthRepository
    {
        bool IsUniqueUser(string username, string email);
        Task<LoginResponseDTO> Login(string usernameOrEmail, string password);
        Task<LocalUser> Register(RegistrationRequestDTO registrationRequestDTO);
        
    }
}
