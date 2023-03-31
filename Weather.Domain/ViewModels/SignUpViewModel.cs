using System.ComponentModel.DataAnnotations;

namespace Weather.Domain.ViewModels
{
    public class SignupViewModel
    {
        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 6)]
        public string? Password { get; set; }

        [Required]
        [Compare("Password")]
        public string? ComparePassword { get; set; }
    }
}
