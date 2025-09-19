using EvaluacionUniversal.Dtos;
using EvaluacionUniversal.Models;

namespace EvaluacionUniversal.Repository
{
    public interface IUserRepository
    {
        Task<UserLoginResponseDto> LogInAsync(UserLoginDto userLoginDto);
        Task<UserLoginResponseDto> ResgisterUserAsync(User user);
    }
}
