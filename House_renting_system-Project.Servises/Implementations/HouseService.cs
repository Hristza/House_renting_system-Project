using House_renting_system_Project.Data.Data;
using House_renting_system_Project.Servises.Contracts;
using House_renting_system_Project.Servises.House;
using House_renting_system_Project.Servises.House.Helpers;
using House_renting_system_Project.Servises.Query;
using Microsoft.EntityFrameworkCore;

namespace House_renting_system_Project.Servises.Implementations
{
    public class HouseService : IHouseService
    {
        private readonly HouseRentingDbContext context;
        public HouseService(HouseRentingDbContext context)
        {
            this.context = context;
        }
        public async Task<AllHouseViewModel> GetAllHousesByQueryAsync(string currentUsersId, QueryViewModel model)
        {
            var allCategories = await context.Categories
                .AsNoTracking()
                .Select(c => new CategoryViewModel
                {
                    Id = c.Id,
                    Name = c.Name,
                })
                .ToListAsync();

            var query = context.Houses
                .AsNoTracking()
                .Where(h => h.IsDeleted == false);

            if (model.CurentCategory != null && model.CurentCategory.Id > 0)
            {
                query = query.Where(h => h.CategoryId == model.CurentCategory.Id);
            }

            if (!string.IsNullOrWhiteSpace(model.KeyWordText))
            {
                query = query.Where(h => h.Title.Contains(model.KeyWordText) ||
                                         h.Address.Contains(model.KeyWordText));
            }

            if (model.IsDecending)
            {
                query = query.OrderByDescending(h => h.Title);
            }
            else
            {
                query = query.OrderBy(h => h.Title);
            }

            var housesViewModel = await query
                .Select(h => new HousesViewModel
                {
                    Id = h.Id,
                    Name = h.Title,
                    Address = h.Address,
                    ImageUrl = h.ImageUrl,
                    CurentUserIsOwner = h.AgentId == currentUsersId,
                    CurrentUserIsRenter = h.RenterId == currentUsersId,
                    IsRented = h.RenterId != null
                })
                .ToListAsync();

            var allHouseViewModel = new AllHouseViewModel
            {
                Houses = housesViewModel,
                Query = new QueryViewModel
                {
                    AllCategory = allCategories,
                    CurentCategory = model.CurentCategory,
                    KeyWordText = model.KeyWordText,
                    IsDecending = model.IsDecending
                }
            };

            return allHouseViewModel;
        }
        public async Task<HouseDitailViewModel> GetDitailAsync(int Id, string userId)
        {
            var searched = await context.Houses
                .Include(h => h.Agent)
                .AsNoTracking()
                .FirstOrDefaultAsync(h => h.Id == Id);

            if (searched == null)
            {
                return null;
            }
            else
            {

                var model = new HouseDitailViewModel()
                {
                    Id = searched.Id,
                    Address = searched.Address,
                    ImageUrl = searched.ImageUrl,
                    Description = searched.Description,
                    CreatedBy = searched.Agent.UserName,
                    Price = searched.PricePerMonth,
                    Name = searched.Title
                };
                return model;
            }
        }
        public async Task<HouseFormViewModel> GetCreateHouseFormAsync()
        {
            List<CategoryViewModel> ListOfCategories = await context.Categories
                .AsNoTracking()
                .Select(c => new CategoryViewModel
                {
                    Id = c.Id,
                    Name = c.Name,
                })
                .ToListAsync();

            var houseCategories = new HouseFormViewModel()
            {
                Categories = ListOfCategories
            };

            return houseCategories;
        }

        public async Task<AllHouseViewModel> GetHousesByUserIdAsync(string userId)
        {
            var houses = await context.Houses
                .AsNoTracking()
                .Where(h => h.AgentId == userId)
                .Select(h => new HousesViewModel
                {
                    Address = h.Address,
                    ImageUrl = h.ImageUrl,
                    Name = h.Title,
                    Id = h.Id,
                    CurentUserIsOwner = true,
                    CurrentUserIsRenter  = h.RenterId == userId,
                    IsRented = h.RenterId != null
                })
                .ToListAsync();

            var allCategories = await context.Categories
                .AsNoTracking()
                .Select(c => new CategoryViewModel
                {
                    Id = c.Id,
                    Name = c.Name,
                })
                .ToListAsync();

            var allHouseViewModel = new AllHouseViewModel
            {
                Houses = houses,
                Query = new QueryViewModel
                {
                    AllCategory = allCategories,
                    IsDecending = false
                }
            };

            return allHouseViewModel;
        }

        public async Task<List<CategoryViewModel>> GetCategoriesAsync()
        {
            return await context.Categories
                .AsNoTracking()
                .Select(c => new CategoryViewModel()
                {
                    Id = c.Id,
                    Name = c.Name,
                })
                .ToListAsync();
        }
        public async Task<string> PostEditHouseAsync(HouseFormViewModel model, string userId)
        {
            var house = await context.Houses.FindAsync(model.Id);
            if (house == null)
            {
                return "404";
            }
            if (house.AgentId != userId)
            {
                return "401";
            }

            house.PricePerMonth = model.PricePerMonth;
            house.Address = model.Address;
            house.ImageUrl = model.ImageUrl;
            house.Description = model.Description;
            house.Title = model.Title;
            house.CategoryId = model.SelectedCategoryId;

            await context.SaveChangesAsync();

            return "202";
        }
        public async Task<string> DeleteHouseAsync(int Id)
        {
            var house = await context.Houses.FindAsync(Id);
            if (house != null && !house.IsDeleted)
            {
                house.IsDeleted = true;
                await context.SaveChangesAsync();
                return "200";
            }
            return "Error: House not found or already deleted";
        }

        public async void PostAddNewHouseAsync(string userId, HouseFormViewModel model)
        {
            

            var newHouse = new Data.Data.Entities.House
            {
                Title = model.Title,
                Address = model.Address,
                Description = model.Description,
                ImageUrl = model.ImageUrl,
                PricePerMonth = model.PricePerMonth,
                CategoryId = model.SelectedCategoryId,
                AgentId = userId,
                IsDeleted = false
            };

            await context.Houses.AddAsync(newHouse);
            await context.SaveChangesAsync();
        }

        public async Task<Data.Data.Entities.House> GetHouseByIdAsync(int Id)
        {
            var house = await context.Houses
                .FirstOrDefaultAsync(h => h.Id == Id);

            if (house == null)
            {
                return null;
            }
            else
            {
                return house;
            }
        }

        public async Task<bool> RentAsync(int id, string userRenterId)
        {
            var house = await context.Houses.FirstOrDefaultAsync(h => h.Id == id);

            if (house is null || house.RenterId is not null)
            {
                return false;
            }

            house.RenterId = userRenterId;
            await context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> LeaveAsync(int id, string userRenterId)
        {
            var house = await context.Houses.FirstOrDefaultAsync(h => h.Id == id);

            if (house is null || house.RenterId is null)
            {
                return false;
            }

            house.RenterId = null;
            await context.SaveChangesAsync();

            return true;
        }
        
    }
}
