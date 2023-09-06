using Microsoft.AspNetCore.Mvc;
using Project.Data.DTO;
using Project.Data.Repository.Interface;
using System.Net;

namespace Project.API.Controllers
{
    [Route("api/UsersAuth")]
    [ApiController]
    
    public class UsersController : Controller
    {
        private readonly IAuthRepository _authRepo;
        protected APIResponse _response;

        public UsersController(IAuthRepository authRepo)
        {
            _authRepo = authRepo;
            _response = new();
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegistrationRequestDTO registrationRequestDTO)
        {
            APIResponse response = new APIResponse(); 

            try
            {
                if (ModelState.IsValid)
                {
                    // Check if the username and email are unique
                    if (!_authRepo.IsUniqueUser(registrationRequestDTO.UserName, registrationRequestDTO.Email))
                    {
                        response.StatusCode = HttpStatusCode.BadRequest;
                        response.IsSuccess = false;
                        response.ErrorMessages = new List<string> { "Username or Email is already in use." };

                        return BadRequest(response);
                    }

                    // Register the user
                    var registeredUser = await _authRepo.Register(registrationRequestDTO);

                    response.StatusCode = HttpStatusCode.OK;
                    response.IsSuccess = true;

                    return Ok("User created successfully");
                }
                else
                {
                    response.StatusCode = HttpStatusCode.BadRequest;
                    response.IsSuccess = false;
                    response.ErrorMessages = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList();

                    return BadRequest(response);
                }
            }
            catch (Exception ex)
            {
                response.StatusCode = HttpStatusCode.InternalServerError;
                response.IsSuccess = false;
                response.ErrorMessages = new List<string> { ex.Message };

                return StatusCode((int)HttpStatusCode.InternalServerError, response);
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO loginRequestDTO)
        {
            APIResponse response;

            try
            {
                var loginResponseDTO = await _authRepo.Login(loginRequestDTO.UsernameOrEmail, loginRequestDTO.Password);

                if (loginResponseDTO.User == null)
                {
                    response = new APIResponse
                    {
                        StatusCode = HttpStatusCode.Unauthorized,
                        IsSuccess = false,
                        ErrorMessages = new List<string> { "Invalid username/email or password." }
                    };

                    return Unauthorized(response);
                }

                response = new APIResponse
                {
                    StatusCode = HttpStatusCode.OK,
                    IsSuccess = true,
                    Result = loginResponseDTO
                };

                return Ok(loginResponseDTO.Token);
            }
            catch (Exception ex)
            {
                response = new APIResponse
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    IsSuccess = false,
                    ErrorMessages = new List<string> { ex.Message }
                };

                return StatusCode((int)HttpStatusCode.InternalServerError, response);
            }
        }


    }
}
