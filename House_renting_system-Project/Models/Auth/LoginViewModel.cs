using System.ComponentModel.DataAnnotations;
using System.Reflection.PortableExecutable;
using System.Runtime.CompilerServices;

namespace House_renting_system_Project.Models.Auth
{
    public class LoginViewModel
    {
        [Required]
        [StringLength(20,MinimumLength = 3, ErrorMessage = "Invalid username")]
        public string Username { get; set; }
        [Required]
        [StringLength(80, MinimumLength = 6, ErrorMessage = "Invalid Password")]
        public string Password { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        public bool RememberMe { get; set; }
    }
}
