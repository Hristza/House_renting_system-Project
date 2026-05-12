using House_renting_system_Project.Data.Migrations;
using System.ComponentModel.DataAnnotations;
using House_renting_system_Project.Extentions;

namespace House_renting_system_Project.Models.Auth
{
    public class RegisterViewModel
    {
        [Required]
        [StringLength(30, MinimumLength = 3)]
        [Display(Name = "Username")]
        public string Username { get; set; } = null!;

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; } = null!;

        [Required]
        [DataType(DataType.Password)]
        [StringLength(100, MinimumLength = 6)]
        [Display(Name = "Password")]
        public string Password { get; set; } = null!;

        [Required]
        [AllowedValues(RoleNames.Agent, RoleNames.Client, ErrorMessage = "Invalid Role Name")]
        public string Role { get; init; } = "";

    }
}
