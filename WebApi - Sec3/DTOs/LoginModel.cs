using System.ComponentModel.DataAnnotations;

namespace WebApi___Sec3.DTOs
{
    public class LoginModel
    {
        [Required(ErrorMessage = "UserName is required")]
        public string? UserName { get; set; }
        [EmailAddress]

        [Required(ErrorMessage = "Password is required")]
        public string? Password { get; set; }

    }
}
