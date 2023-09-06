using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Project.Data.Context;
using Project.Data.DTO;
using Project.Data.Models;
using Project.Data.Repository.Interface;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Project.Data.Repository.Implementation
{
    public class AuthRepository : IAuthRepository
    {
        private readonly ApplicationDbContext _db;
        private string secretKey;
        

        public AuthRepository(ApplicationDbContext db, IConfiguration configuration)
        {
            _db = db;
            secretKey = configuration.GetValue<string>("ApiSettings:Secret");
            
        }
        public bool IsUniqueUser(string username, string email)
        {
            var user = _db.LocalUsers.FirstOrDefault(x => x.UserName == username || x.Email == email);
            if (user == null)
            {
                return true;
            }
            return false;
        }

        public async Task<LocalUser> Register(RegistrationRequestDTO registrationRequestDTO)
        {
            bool isUsernameTaken = await _db.LocalUsers.AnyAsync(u => u.UserName == registrationRequestDTO.UserName);
            bool isEmailTaken = await _db.LocalUsers.AnyAsync(u => u.Email == registrationRequestDTO.Email);

            if (isUsernameTaken)
            {
                throw new Exception("Username is already in use.");
            }

            if (isEmailTaken)
            {
                throw new Exception("Email is already in use.");
            }

            LocalUser user = new()
            {
                UserName = registrationRequestDTO.UserName,
                Email = registrationRequestDTO.Email,
                Password = registrationRequestDTO.Password
            };

            _db.LocalUsers.Add(user);
            await _db.SaveChangesAsync();
            user.Password = "";
            return user;
        }

        public async Task<LoginResponseDTO> Login(string usernameOrEmail, string password)
        {
           
            var users = await _db.LocalUsers
                .Where(u => (u.UserName == usernameOrEmail || u.Email == usernameOrEmail))
                .ToListAsync(); 

            var user = users.FirstOrDefault(u => string.Equals(u.Password, password, StringComparison.Ordinal));


            if (user == null)
            {
                return new LoginResponseDTO()
                {
                    Token = "",
                    User = null
                };
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secretKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString()),

                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            LoginResponseDTO loginResponseDTO = new LoginResponseDTO()
            {
                Token = tokenHandler.WriteToken(token),
                User = user,
            };
            return loginResponseDTO;
        }


    }
}
