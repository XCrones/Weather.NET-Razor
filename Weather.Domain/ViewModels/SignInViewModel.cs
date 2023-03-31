using System.ComponentModel.DataAnnotations;

namespace Weather.Domain.ViewModels
{
    public class SigninViewModel
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 6)]
        public string Password { get; set; }
    }
}
