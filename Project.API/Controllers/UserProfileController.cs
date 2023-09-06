using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project.Core.Services;
using Project.Data.DTO;
using Project.Data.Models;

namespace Project.API.Controllers
{
    [ApiController]
    [Route("api/UserProfiles")]
    public class UserProfileController : ControllerBase
    {

        private readonly IUserProfileService _userService;
        private readonly IMapper _mapper;

        public IUserProfileService Object { get; }

        public UserProfileController(IUserProfileService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }
        [Authorize]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ResponseDTO<UserProfileDTO>>> CreateUserProfile(UserProfileDTO profile)
        {
            try
            {
                var profiles = _mapper.Map<UserProfile>(profile);
                var response = await _userService.CreateProfile(profiles);

                if (response.StatusCode == 200)
                    return Ok(response);
                else
                    return BadRequest(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [Authorize]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ResponseDTO<List<UserProfileDTO>>>> GetAllProfiles()
        {
            try
            {
                var response = await _userService.GetAllProfiles();

            if (response.StatusCode == 200)
                return Ok(response);
            else
                return BadRequest(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }

        [Authorize]
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ResponseDTO<UserProfileDTO>>> GetProfileById(int id)
        {
            var response = await _userService.GetProfileById(id);
            if (response.StatusCode == 200)
            {
                return Ok(response);
            }
            else
            {
                return NotFound(response);
            }
        }

        [Authorize]
        [HttpDelete("{id}/delete")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ResponseDTO<UserProfileDTO>>> DeleteProfilel(int id)
        {
            var result = await _userService.DeleteProfile(id);
            if (result.StatusCode == 200)
            {
                return Ok(result);
            }
            else if (result.StatusCode == 404)
            {
                return NotFound(result);
            }
            else
            {
                return BadRequest(result);
            }
        }
        [Authorize]
        [HttpPut("{id}/update")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateUserProfile([FromBody] UserProfileDTO profile)
        {
            if (profile == null)
            {
                return BadRequest("Invalid request data.");
            }

            var result = await _userService.UpdateProfile(profile);

            if (result)
            {
                return Ok("Profile updated successfully.");
            }
            else
            {
                return StatusCode(500, "Failed to update profile."); 
            }
        }
    }
}
