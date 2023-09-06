using Project.Data.DTO;
using Project.Data.Models;
using Project.Data.Repository.Interface;

namespace Project.Core.Services.Implementation
{
    public class UserProfileService : IUserProfileService
    {
        private readonly IUserProfileRepository _userRepo;
        public UserProfileService(IUserProfileRepository userRepo)
        {
            _userRepo = userRepo;
        }
        public async Task<ResponseDTO<UserProfileDTO>> CreateProfile(UserProfile profile)
        {
            return await _userRepo.CreateProfile(profile);
        }

        public async Task<ResponseDTO<UserProfileDTO>> DeleteProfile(int id)
        {
            return await _userRepo.DeleteProfile(id);
        }

        public async Task<ResponseDTO<List<UserProfileDTO>>> GetAllProfiles()
        {
            return await _userRepo.GetAllProfiles();
        }

        public async Task<ResponseDTO<UserProfileDTO>> GetProfileById(int id)
        {
            return await _userRepo.GetProfileById(id);
        }

        public async Task<bool> UpdateProfile(UserProfileDTO profile)
        {
          return await _userRepo.UpdateProfile(profile);
        }
        
    }
}
