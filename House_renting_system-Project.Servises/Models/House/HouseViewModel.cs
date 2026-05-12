using House_renting_system_Project.Servises.Query;

namespace House_renting_system_Project.Servises.House
{
    public class HousesViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string ImageUrl { get; set; }
        public bool CurentUserIsOwner { get; set; }
        public bool CurrentUserIsRenter { get; init; }
        public bool IsRented { get; init; }

    }
}
