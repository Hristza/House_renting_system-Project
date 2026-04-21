using House_renting_system_Project.Data.Data;
using House_renting_system_Project.Data.Data.Entities;
using House_renting_system_Project.Models.House;
using House_renting_system_Project.Models.House.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Security.Claims;

namespace House_renting_system_Project.Controllers
{
    public class HouseController : Controller
    {
        private readonly HouseRentingDbContext context;

        public HouseController(HouseRentingDbContext context)
        {
            this.context = context;
        }
        [HttpGet]
        public async Task<IActionResult> AllHouses()
        {
            var currentUsersId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var housesViewModel = await context.Houses
            .AsNoTracking()
            .Select(h => new HousesViewModel
            {
                Id = h.Id,
                Name = h.Title,
                Address = h.Address,
                ImageUrl = h.ImageUrl,
                CurentUserIsOwner = h.AgentId == currentUsersId
            })
            .ToListAsync();
            ViewBag.Title = "All houses";
            return View(housesViewModel);
        }
        [HttpGet]
        public async Task<IActionResult> Details(int Id) 
        {
            var searched = await context.Houses
                .Include(h => h.Agent)
                .AsNoTracking()
                .FirstOrDefaultAsync(h => h.Id == Id);

            var model = new HouseDitailViewModel()
            {
                Id = searched.Id,
                Address = searched.Address,
                ImageUrl = searched.ImageUrl,
                Description = searched.Description,
                CreatedBy = searched.Agent.UserName,
                Price =searched.PricePerMonth,
                Name = searched.Title
            };

            return View(model);
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> CreateHouse()
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
            return View(houseCategories);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateHouse(HouseFormViewModel model)
        {

            var houseCategories = await context.Categories
                .AsNoTracking()
                .Select(c => new CategoryViewModel()
                {
                    Id = c.Id,
                    Name = c.Name,
                })
                .ToListAsync();

            if (!ModelState.IsValid)
            {
                
                model.Categories = houseCategories;
                return View(model);
            }
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            bool addressExists = await context.Houses
                .AnyAsync(h => h.Address.ToLower() == model.Address.ToLower());

            if (addressExists)
            {
                model.Categories = houseCategories;
                ModelState.AddModelError("Address", "This address is already registered");
                return View(model);
            }

            var newHouse = new House
            {
                Title = model.Title,
                Address = model.Address,
                Description = model.Description,
                ImageUrl = model.ImageUrl,
                PricePerMonth = model.PricePerMonth,
                CategoryId = model.SelectedCategoryId,
                AgentId = userId
            };

            context.Houses.Add(newHouse);
            await context.SaveChangesAsync();

            return RedirectToAction(nameof(AllHouses));
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> MyHouses()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var houses = context.Houses
                .Where(h => h.AgentId == userId)
                .Select(h => new HousesViewModel
                {
                    Address = h.Address,
                    ImageUrl = h.ImageUrl,
                    Name = h.Title,
                    Id = h.Id,
                    CurentUserIsOwner = true
                })
                .ToListAsync();
            ViewBag.Title = "My houses";
            return View(nameof(AllHouses), houses);
        }
    }
}