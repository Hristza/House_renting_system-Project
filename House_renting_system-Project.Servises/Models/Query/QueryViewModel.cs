using House_renting_system_Project.Servises.House.Helpers;

namespace House_renting_system_Project.Servises.Query
{
    public class QueryViewModel
    {
        public List<CategoryViewModel>? AllCategory { get; set; }

        public CategoryViewModel? CurentCategory { get; set; }

        public string? KeyWordText { get; set; }

        public bool IsDecending { get; set; }
    }
}