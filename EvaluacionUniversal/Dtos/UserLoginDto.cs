using System.ComponentModel.DataAnnotations;

namespace EvaluacionUniversal.Dtos
{
    public class UserLoginDto
    {
        [Required(ErrorMessage = "el email es obligatorio")]
        public string Email { get; set; }
        [Required(ErrorMessage = "el password es obligatorio")]
        public string Password { get; set; }
    }
}
