using House_renting_system_Project.Data.Data;
using House_renting_system_Project.Servises.Contracts;
using House_renting_system_Project.Servises.House;
using House_renting_system_Project.Servises.Query;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using House_renting_system_Project.Extentions;

namespace House_renting_system_Project.Controllers
{
    public class HouseController : Controller
    {
        private readonly HouseRentingDbContext context;
        private readonly IHouseService houseService;

        public HouseController(HouseRentingDbContext context,
            IHouseService houseService)
        {
            this.context = context;
            this.houseService = houseService;
        }

        [HttpGet]
        public async Task<IActionResult> AllHouses([FromQuery] QueryViewModel model)
        {
            var currentUsersId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var allHouseViewModel = await houseService.GetAllHousesByQueryAsync(currentUsersId, model);

            ViewBag.Title = "All houses";
            return View(allHouseViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int Id)
        {
            var model = await houseService.GetDitailAsync(Id, this.User.GetId());
            if (model == null)
            {
                return RedirectToAction("Error404", "Home");
            }
            else
            {
                return View(model);
            }
        }

        [HttpGet]
        [Authorize(Roles = RoleNames.Agent)]
        public async Task<IActionResult> CreateHouse()
        {
            var houseForm = await houseService.GetCreateHouseFormAsync();
            return View(houseForm);
        }

        [HttpPost]
        [Authorize(Roles = RoleNames.Agent)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateHouse(HouseFormViewModel model)
        { 
            if (!ModelState.IsValid)
            {
                var houseCategories = await houseService.GetCategoriesAsync();
                model.Categories = houseCategories;

                var errors = ModelState.Values.SelectMany(v => v.Errors);
                foreach (var error in errors)
                {
                    ModelState.AddModelError(string.Empty, error.ErrorMessage);
                }
                return View(model);
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            bool addressExists = await context.Houses
                .AnyAsync(h => h.Address.ToLower() == model.Address.ToLower() && h.IsDeleted == false);

            if (addressExists)
            {
                var houseCategories = await houseService.GetCategoriesAsync();
                model.Categories = houseCategories;
                ModelState.AddModelError("Address", "This address is already registered");
                return View(model);
            }

            houseService.PostAddNewHouseAsync(userId, model);

            TempData["SuccessMessage"] = "House created successfully!";
            return RedirectToAction(nameof(AllHouses));
        }

        [HttpGet]
        [Authorize(Roles = RoleNames.Agent)]
        public async Task<IActionResult> MyHouses()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var allHouseViewModel = await houseService.GetHousesByUserIdAsync(userId);

            ViewBag.Title = "My houses";
            return View(nameof(AllHouses), allHouseViewModel);
        }

        [HttpGet]
        [Authorize(Roles = RoleNames.Agent)]
        public async Task<IActionResult> Edit(int id)
        {
            var house = houseService.GetHouseByIdAsync(id);

            if (house == null)
            {
                return RedirectToAction("Error404", "Home");
            }

            var houseCategories = await houseService.GetCategoriesAsync();

            var model = new HouseFormViewModel()
            {
                Id = house.Id,
                Address = house.Result.Address,
                ImageUrl = house.Result.ImageUrl,
                Description = house.Result.Description,
                Title = house.Result.Title,
                PricePerMonth = house.Result.PricePerMonth,
                Categories = houseCategories,
                SelectedCategoryId = house.Result.CategoryId
            };
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = RoleNames.Agent)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(HouseFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var houseCategories = await houseService.GetCategoriesAsync();
                model.Categories = houseCategories;

                var errors = ModelState.Values.SelectMany(v => v.Errors);
                foreach (var error in errors)
                {
                    ModelState.AddModelError(string.Empty, error.ErrorMessage);
                }

                return View(model);
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var house = await houseService.PostEditHouseAsync(model, userId);
            if (house == "404")
            {
                return RedirectToAction("Error404", "Home");
            }
            if (house == "401")
            {
                return RedirectToAction("Error401", "Home");
            }

            TempData["SuccessMessage"] = "House updated successfully!";
            return RedirectToAction(nameof(MyHouses));
        }

        [Authorize(Roles = RoleNames.Agent)]
        public async Task<IActionResult> Delete(int id)
        {
            var delite = await houseService.DeleteHouseAsync(id);
            return RedirectToAction(nameof(MyHouses));
        }

        [Authorize(Roles = RoleNames.Client)]
        public async Task<IActionResult> Rent(int id)
        {
            var currentUserId = this.User.GetId();
            if (string.IsNullOrWhiteSpace(currentUserId))
            {
                return this.Unauthorized();
            }
            var succes = await houseService.RentAsync(id, currentUserId);
            if (succes)
            {
                return this.RedirectToAction(nameof(this.AllHouses));
            }
            return this.Unauthorized();
        }
        [Authorize(Roles = RoleNames.Client)]
        public async Task<IActionResult> Leave(int id)
        {
            var currentUserId = this.User.GetId();
            if (string.IsNullOrWhiteSpace(currentUserId))
            {
                return this.Unauthorized();
            }
            var succes = await houseService.LeaveAsync(id, currentUserId);
            if (succes)
            {
                return this.RedirectToAction(nameof(this.AllHouses));
            }
            return this.Unauthorized();
        }
    }
}