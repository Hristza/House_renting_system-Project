using System.Reflection.Metadata.Ecma335;

namespace House_renting_system_Project.Models.House
{
    public class HouseDitailViewModel: HousesViewModel
    {
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string CreatedBy { get; set; }
    }
}
