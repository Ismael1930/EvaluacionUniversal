using System.ComponentModel.DataAnnotations;

namespace EvaluacionUniversal.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage="El nombre es obligatorio")]
        public string Name { get; set; }

        [Required(ErrorMessage="El correo es obligatorio")]
        [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "El formato del correo no es válido.")]
        public string Email { get; set; }

        [Required(ErrorMessage="La contraseña es obligatoria")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*[\W_]).{8,}$", ErrorMessage = "La contraseña debe tener al menos 8 caracteres, una mayúscula, una minúscula y un símbolo.")]
        public string Password { get; set; }
    }
}
