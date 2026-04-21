using System.ComponentModel.DataAnnotations;

namespace House_renting_system_Project.Models.House.Helpers
{
    public class CategoryViewModel
    {
        public int Id { get; init; }
        public string Name { get; set; } = null!;
    }
}
