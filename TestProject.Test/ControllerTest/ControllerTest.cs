using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Project.API.Controllers;
using Project.Core.Services;
using Project.Data.DTO;
using Project.Data.Models;
using Project.Data.Repository.Interface;

namespace TestProject.Test.ControllerTest.ControllerTest
{


    public class UsersControllerTests
    {
        private readonly UserProfileController _controller;
        private readonly Mock<IUserProfileService> _userServiceMock;
        private readonly Mock<IMapper> _mapperMock;

        public UsersControllerTests()
        {
            _userServiceMock = new Mock<IUserProfileService>();
            _mapperMock = new Mock<IMapper>();
            _controller = new UserProfileController(_userServiceMock.Object, _mapperMock.Object);

        }

        [Fact]
        public async Task Register_ValidModel_ReturnsOkResult()
        {
            // Arrange
            var mockAuthRepo = new Mock<IAuthRepository>();
            mockAuthRepo.Setup(repo => repo.IsUniqueUser(It.IsAny<string>(), It.IsAny<string>())).Returns(true);
            mockAuthRepo.Setup(repo => repo.Register(It.IsAny<RegistrationRequestDTO>())).ReturnsAsync(new LocalUser());

            var controller = new UsersController(mockAuthRepo.Object);

            // Act
            var result = await controller.Register(new RegistrationRequestDTO { UserName = "TestUser", Email = "test@example.com" });

            // Assert
            Assert.IsType<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.Equal("User created successfully", okResult.Value);
        }

        [Fact]
        public async Task Register_InvalidModel_ReturnsBadRequest()
        {
            // Arrange
            var mockAuthRepo = new Mock<IAuthRepository>();
            var controller = new UsersController(mockAuthRepo.Object);
            controller.ModelState.AddModelError("FieldName", "Error message");

            // Act
            var result = await controller.Register(new RegistrationRequestDTO());

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
            var badRequestResult = result as BadRequestObjectResult;
            Assert.NotNull(badRequestResult.Value);
        }

        [Fact]
        public async Task Login_ValidCredentials_ReturnsOkResult()
        {
            // Arrange
            var mockAuthRepo = new Mock<IAuthRepository>();
            var user = new LocalUser { Id = 1, UserName = "TestUser", Email = "test@example.com" }; ; // Replace User with your actual user type
            var loginResponseDTO = new LoginResponseDTO { User = user, Token = "TestToken" };
            mockAuthRepo.Setup(repo => repo.Login(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(loginResponseDTO);

            var controller = new UsersController(mockAuthRepo.Object);
            var loginRequestDTO = new LoginRequestDTO { UsernameOrEmail = "TestUser", Password = "TestPassword" };

            // Act
            var result = await controller.Login(loginRequestDTO);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.Equal("TestToken", okResult.Value);
        }

        [Fact]
        public async Task Login_InvalidCredentials_ReturnsUnauthorized()
        {
            // Arrange
            var mockAuthRepo = new Mock<IAuthRepository>();
            mockAuthRepo.Setup(repo => repo.Login(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(new LoginResponseDTO());

            var controller = new UsersController(mockAuthRepo.Object);
            var loginRequestDTO = new LoginRequestDTO { UsernameOrEmail = "InvalidUser", Password = "InvalidPassword" };

            // Act
            var result = await controller.Login(loginRequestDTO);

            // Assert
            Assert.IsType<UnauthorizedObjectResult>(result);
        }

        [Fact]
        public async Task CreateUserProfile_ValidData_ReturnsOk()
        {
            // Arrange
            var userProfileDTO = new UserProfileDTO
            {
                UserId = 1,
                UserName = "boy",
                Email = "c@gmail.com",
            };
            var userProfile = new UserProfile
            {

                UserName = "boy",
                Email = "c@gmail.com",
            };

            _mapperMock.Setup(m => m.Map<UserProfile>(userProfileDTO)).Returns(userProfile);

            var responseDto = new ResponseDTO<UserProfileDTO>
            {
                StatusCode = 200,
                Result = userProfileDTO
            };

            _userServiceMock.Setup(s => s.CreateProfile(userProfile)).ReturnsAsync(responseDto);

            // Act
            var result = await _controller.CreateUserProfile(userProfileDTO);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var response = Assert.IsType<ResponseDTO<UserProfileDTO>>(okResult.Value);
            Assert.Equal(200, response.StatusCode);
            Assert.Equal(userProfileDTO, response.Result);
        }

        [Fact]
        public async Task CreateUserProfile_InvalidData_ReturnsBadRequest()
        {
            // Arrange
            var invalidUserProfileDTO = new UserProfileDTO
            {
                UserId = 0,
                UserName = null,
                Email = "invalid-email",
            };

            var responseDto = new ResponseDTO<UserProfileDTO>
            {
                StatusCode = 400,
                Result = null
            };

            _userServiceMock.Setup(s => s.CreateProfile(It.IsAny<UserProfile>())).ReturnsAsync(responseDto);

            // Act
            var result = await _controller.CreateUserProfile(invalidUserProfileDTO);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            var response = Assert.IsType<ResponseDTO<UserProfileDTO>>(badRequestResult.Value);
            Assert.Equal(400, response.StatusCode);

        }

        [Fact]
        public async Task GetAllProfiles_ReturnsOk()
        {
            // Arrange
            var userProfileList = new List<UserProfileDTO> { };

            var responseDto = new ResponseDTO<List<UserProfileDTO>>
            {
                StatusCode = 200,
                Result = userProfileList
            };

            _userServiceMock.Setup(s => s.GetAllProfiles()).ReturnsAsync(responseDto);

            // Act
            var result = await _controller.GetAllProfiles();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var response = Assert.IsType<ResponseDTO<List<UserProfileDTO>>>(okResult.Value);
            Assert.Equal(200, response.StatusCode);
            Assert.Equal(userProfileList, response.Result);
        }

        [Fact]
        public async Task GetProfileById_NonExistingId_ReturnsNotFound()
        {
            // Arrange
            var userId = 999;

            var responseDto = new ResponseDTO<UserProfileDTO>
            {
                StatusCode = 404,
                Result = null
            };

            _userServiceMock.Setup(s => s.GetProfileById(userId)).ReturnsAsync(responseDto);

            // Act
            var result = await _controller.GetProfileById(userId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            var response = Assert.IsType<ResponseDTO<UserProfileDTO>>(notFoundResult.Value);
            Assert.Equal(404, response.StatusCode);
            Assert.Null(response.Result);
        }

        [Fact]
        public async Task DeleteProfile_ExistingId_ReturnsOk()
        {
            // Arrange
            var userId = 1;

            var responseDto = new ResponseDTO<UserProfileDTO>
            {
                StatusCode = 200,
                Result = null
            };

            _userServiceMock.Setup(s => s.DeleteProfile(userId)).ReturnsAsync(responseDto);

            // Act
            var result = await _controller.DeleteProfilel(userId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var response = Assert.IsType<ResponseDTO<UserProfileDTO>>(okResult.Value);
            Assert.Equal(200, response.StatusCode);
            Assert.Null(response.Result);
        }

        [Fact]
        public async Task DeleteProfile_NonExistingId_ReturnsNotFound()
        {
            // Arrange
            var userId = 999;

            var responseDto = new ResponseDTO<UserProfileDTO>
            {
                StatusCode = 404,
                Result = null
            };

            _userServiceMock.Setup(s => s.DeleteProfile(userId)).ReturnsAsync(responseDto);

            // Act
            var result = await _controller.DeleteProfilel(userId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            var response = Assert.IsType<ResponseDTO<UserProfileDTO>>(notFoundResult.Value);
            Assert.Equal(404, response.StatusCode);
            Assert.Null(response.Result);
        }

        [Fact]
        public async Task UpdateUserProfile_ValidData_ReturnsOk()
        {
            // Arrange
            var userProfileDTO = new UserProfileDTO { };


            _userServiceMock.Setup(s => s.UpdateProfile(userProfileDTO)).ReturnsAsync(true);

            // Act
            var result = await _controller.UpdateUserProfile(userProfileDTO);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Profile updated successfully.", okResult.Value);
        }

        [Fact]
        public async Task UpdateUserProfile_InvalidData_ReturnsInternalServerError()
        {
            // Arrange
            var invalidUserProfileDTO = new UserProfileDTO { };


            _userServiceMock.Setup(s => s.UpdateProfile(invalidUserProfileDTO)).ReturnsAsync(false);

            // Act
            var result = await _controller.UpdateUserProfile(invalidUserProfileDTO);

            // Assert
            if (result is ObjectResult objectResult)
            {

                Assert.Equal(500, objectResult.StatusCode);
                Assert.Equal("Failed to update profile.", objectResult.Value);
            }
            else if (result is StatusCodeResult statusCodeResult)
            {

                Assert.Equal(500, statusCodeResult.StatusCode);
            }
            else
            {

                Assert.True(false, $"Unexpected result type: {result.GetType()}");
            }




        }
    }
}



