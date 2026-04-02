using House_renting_system_Project.Data.Data;
using House_renting_system_Project.Data.Data.Entities;
using House_renting_system_Project.Models;
using House_renting_system_Project.Models.House;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

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
            var housesViewModel = await context.Houses
            .Select(h => new Models.HousesViewModel
            {
                Id = h.Id,
                Name = h.Title,
                Address = h.Address,
                ImageUrl = h.ImageUrl
            })
            .ToListAsync();
            return View(housesViewModel);
        }
        [HttpGet]
        public async Task<IActionResult> Details(int Id)
        {
            var searched = await context.Houses.FirstOrDefaultAsync(h => h.Id == Id);
            return View(searched);
        }
        public IActionResult CreateHouse()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateHouse(HouseFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            bool addressExists = await context.Houses
                .AnyAsync(h => h.Address.ToLower() == model.Address.ToLower());

            if (addressExists)
            {
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
                // CategoryId = model.CategoryId,
                // AgentId = model.AgentId
            };

            context.Houses.Add(newHouse);
            await context.SaveChangesAsync();

            return RedirectToAction("AllHouses");
        }
    }
}