using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Project.Data.Context;
using Project.Data.DTO;
using Project.Data.Models;
using Project.Data.Repository.Interface;

namespace Project.Data.Repository.Implementation
{
    public class UserProfileRepository : IUserProfileRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;
        public UserProfileRepository(ApplicationDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }
        public async Task<ResponseDTO<UserProfileDTO>> CreateProfile(UserProfile profile)
        {
           var userProfileDTO = _mapper.Map<UserProfileDTO>(profile);
            var response = new ResponseDTO<UserProfileDTO>();
            try
            {
                _db.UserProfiles.Add(userProfileDTO);
                await _db.SaveChangesAsync();
                response.StatusCode = 200;
                response.DisplayMessage = "User profile successfully created";
                response.Result = userProfileDTO;
            }
            catch (DbUpdateException ex)
            {

                response.StatusCode = 400;
                response.DisplayMessage = (ex.Message);

            }

            return response;
        }

        public async Task<ResponseDTO<UserProfileDTO>> DeleteProfile(int id)
        {
            var response = new ResponseDTO<UserProfileDTO>();
            var checksaving = await _db.UserProfiles.FindAsync(id);
            if (checksaving == null)
            {
                response.DisplayMessage = "Error";
                response.StatusCode = StatusCodes.Status404NotFound;
                return response;
            }

            var delete = _db.UserProfiles.Remove(checksaving);
            var changes = await _db.SaveChangesAsync();
            if (changes > 0)
            {
                response.StatusCode = StatusCodes.Status200OK;
                response.DisplayMessage = "Successful";
                return response;
            }
            response.DisplayMessage = "Error";
            response.StatusCode = StatusCodes.Status501NotImplemented;
            return response;
        }

        public async Task<ResponseDTO<List<UserProfileDTO>>> GetAllProfiles()
        {
            var response = new ResponseDTO<List<UserProfileDTO>>();
            try
            {

                var profiles = await _db.UserProfiles.ToListAsync();
                var userProfileDTOs = _mapper.Map<List<UserProfileDTO>>(profiles);

                response.StatusCode = 200;
                response.DisplayMessage = "All User profiles retrieved.";
                response.Result = userProfileDTOs;
            }
            catch (DbUpdateException ex)
            {

                response.StatusCode = 400;
                response.DisplayMessage = (ex.Message);

            }

            return response;
        }

        public async Task<ResponseDTO<UserProfileDTO>> GetProfileById(int id)
        {
            var response = new ResponseDTO<UserProfileDTO>();
            try
            {
                var profile = await _db.UserProfiles.FindAsync(id);
                var profiles = _mapper.Map<UserProfileDTO>(profile);
                if (profile != null)
                {
                    response.StatusCode = 200;
                    response.DisplayMessage = "User profile found";
                    response.Result = profiles;
                }
                else
                {
                    response.StatusCode = 404;
                    response.DisplayMessage = "User profile not found";
                }
            }
            catch (Exception ex)
            {
                response.StatusCode = 400;
                response.DisplayMessage = (ex.Message);
            }

            return response;
        }

        public async Task<bool> UpdateProfile(UserProfileDTO profile)
        {
            var updateProfile = _db.UserProfiles.Update(profile);
            var saveTarget = await _db.SaveChangesAsync();
            if (saveTarget > 0)
            {
                return true;
            }
            return false;
        }
    }
}
