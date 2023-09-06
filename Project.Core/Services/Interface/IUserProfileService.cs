using Project.Data.DTO;
using Project.Data.Models;

namespace Project.Core.Services
{
    public interface IUserProfileService
    {
        Task<ResponseDTO<UserProfileDTO>> CreateProfile(UserProfile profile);

        Task<ResponseDTO<List<UserProfileDTO>>> GetAllProfiles();

        Task<ResponseDTO<UserProfileDTO>> GetProfileById(int id);

        Task<bool> UpdateProfile(UserProfileDTO profile);

        Task<ResponseDTO<UserProfileDTO>> DeleteProfile(int id);
    }
}
