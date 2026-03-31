using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using static House_renting_system_Project.Data.Data.DataConstants.Category;

namespace House_renting_system_Project.Data.Data.Entities
{
    public class Category
    {
        public int Id { get; init; }
        [Required]
        [MaxLength(NameMaxLength)]
        public string Name { get; set; } = null!;
        public IEnumerable<House> Houses { get; init; } = new List<House>();
    }
}
