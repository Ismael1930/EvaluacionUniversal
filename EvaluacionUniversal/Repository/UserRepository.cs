using EvaluacionUniversal.Data;
using EvaluacionUniversal.Dtos;
using EvaluacionUniversal.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EvaluacionUniversal.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;
        private string SecretKey;

        public UserRepository(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            SecretKey = configuration.GetValue<string>("ApiSettings:Secret");
        }

        public async Task<UserLoginResponseDto> LogInAsync(UserLoginDto userLoginDto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == userLoginDto.Email);

            if (user == null || !BCrypt.Net.BCrypt.Verify(userLoginDto.Password, user.Password))
                return new UserLoginResponseDto()
                {
                    Token = "",
                    Email = null,
                    UserName = null,
                    Id = null
                };

            return BuildToken(user);
        }

        public async Task<UserLoginResponseDto> ResgisterUserAsync(User user)
        {
            var Emailexist = await _context.Users.AnyAsync(u => u.Email == user.Email);
            if (Emailexist)
                return new UserLoginResponseDto
                {
                    Token = "",
                    Email = null,
                    UserName = null,
                    Id = null
                };


            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return BuildToken(user);
        }

        private UserLoginResponseDto BuildToken(User user)
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Email, user.Email)

            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(SecretKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            UserLoginResponseDto UserLoginResponseDto = new UserLoginResponseDto()
            {
                Token = tokenHandler.WriteToken(token),
                UserName = user.Name,
                Email = user.Email,
                Id = user.Id.ToString()
            };

            return UserLoginResponseDto;
        }
    }
}
