namespace House_renting_system_Project.Models.House
{
    internal class HouseDetailsViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Address { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public decimal PricePerMonth { get; set; }
    }
}