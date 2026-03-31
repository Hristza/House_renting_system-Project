using Microsoft.AspNetCore.Identity;
using static House_renting_system_Project.Data.Data.DataConstants.House;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Net.Cache;
using System.Reflection.Metadata.Ecma335;
using System.Reflection.PortableExecutable;
using System.Security.Principal;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;

namespace House_renting_system_Project.Data.Data.Entities
{
    public class House
    {
        [Key]
        public int Id { get; init; }

        [MaxLength(TitleMaxLength)]
        [MinLength(10)]
        [Required]
        public string Title { get; set; } = null!;

        [MaxLength(AddressMaxLength)]
        [MinLength(30)]
        [Required]
        public string Address { get; set; } = null!;

        [MaxLength(DescriptionMaxLength)]
        [MinLength(50)]
        [Required]
        public string Description { get; set; } = null!;

        [Required]
        public string ImageUrl { get; set; } = null!;

        [MaxLength(2000)]
        [Required]
        [Column(TypeName = "decimal(12, 3)")]
        public decimal PricePerMonth { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; init; } = null!;

        public int AgentId { get; set; }
        public Agent Agent { get; set; }
        public string? RenterId { get; set; }
        public ApplicationUser? Renter { get; set; }
        


    }
}