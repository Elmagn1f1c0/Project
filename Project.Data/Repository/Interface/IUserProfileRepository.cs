using Project.Data.DTO;
using Project.Data.Models;

namespace Project.Data.Repository.Interface
{
    public interface IUserProfileRepository
    {
        Task<ResponseDTO<UserProfileDTO>> CreateProfile(UserProfile profile);

        Task<ResponseDTO<List<UserProfileDTO>>> GetAllProfiles();

        Task<ResponseDTO<UserProfileDTO>> GetProfileById(int id);

        Task<bool> UpdateProfile(UserProfileDTO profile);

        Task<ResponseDTO<UserProfileDTO>> DeleteProfile(int id);
    }
}
