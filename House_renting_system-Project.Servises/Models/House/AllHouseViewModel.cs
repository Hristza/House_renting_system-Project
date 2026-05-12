using House_renting_system_Project.Servises.Query;

namespace House_renting_system_Project.Servises.House
{
    public class AllHouseViewModel
    {
        public QueryViewModel Query { get; set; }
        public List<HousesViewModel> Houses { get; set; }

    }
}
