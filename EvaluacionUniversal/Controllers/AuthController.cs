using EvaluacionUniversal.Dtos;
using EvaluacionUniversal.Models;
using EvaluacionUniversal.Repository;
using Microsoft.AspNetCore.Mvc;

namespace EvaluacionUniversal.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class AuthController : ControllerBase
    {
        private readonly IUserRepository _repo;

        public AuthController(IUserRepository repo)
        {
            _repo = repo;
        }

        [HttpPost("registro")]
        public async Task<IActionResult> Registrar([FromBody] User user)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _repo.ResgisterUserAsync(user);

            if (string.IsNullOrEmpty(result.Token))
                return BadRequest("El correo ya está registrado.");

            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDto dto)
        {
            var result = await _repo.LogInAsync(dto);

            if (string.IsNullOrEmpty(result.Token))
                return Unauthorized("Credenciales inválidas.");

            return Ok(result);
        }

    }
}
