using House_renting_system_Project.Data.Data.Entities;
using House_renting_system_Project.Servises.House;
using House_renting_system_Project.Servises.House.Helpers;
using House_renting_system_Project.Servises.Query;

namespace House_renting_system_Project.Servises.Contracts
{
    public interface IHouseService
    {
        Task<AllHouseViewModel> GetAllHousesByQueryAsync(string userId, QueryViewModel model);
        Task<HouseDitailViewModel> GetDitailAsync(int Id, string userId);
        Task<HouseFormViewModel> GetCreateHouseFormAsync();
        void PostAddNewHouseAsync(string userId, HouseFormViewModel model);
        Task<AllHouseViewModel> GetHousesByUserIdAsync(string userId);
        Task<string> PostEditHouseAsync(HouseFormViewModel model, string userId);
        Task<string> DeleteHouseAsync(int Id);
        Task<List<CategoryViewModel>> GetCategoriesAsync();
        Task<Data.Data.Entities.House> GetHouseByIdAsync(int Id);
        Task<bool> RentAsync(int id, string userRenterId);
        Task<bool> LeaveAsync(int id, string userRenterId);
    }
}
