using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.Principal;
using System.Text;
using static House_renting_system_Project.Data.Data.DataConstants.Agent;

namespace House_renting_system_Project.Data.Data.Entities
{
    public class Agent
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(PhoneNumberMaxLength)]
        [Required]
        public string PhoneNumber { get; set; } = null!;
        [Required]
        public string UserId { get; set; } = null!;
        public ApplicationUser User { get; set; } = null!;
        public IEnumerable<House> ManagedHouses { get; set; } = new List<House>();
    }
}
