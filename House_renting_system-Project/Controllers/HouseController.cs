using House_renting_system_Project.Data.Data;
using House_renting_system_Project.Data.Data.Entities;
using House_renting_system_Project.Models;
using House_renting_system_Project.Models.House;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace House_renting_system_Project.Controllers
{
    public class HouseController : Controller
    {
        private readonly HouseRentingDbContext _db;

        public HouseController(HouseRentingDbContext databaseInstance)
        {
            _db = databaseInstance;
        }

        [HttpGet]
        public async Task<IActionResult> AllHouses()
        {
            var allProperties = await _db.Houses
                .Select(h => new Models.HousesViewModel
                {
                    Id = h.Id,
                    Name = h.Title,
                    Address = h.Address,
                    ImageUrl = h.ImageUrl
                })
                .ToListAsync();

            return View(allProperties);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var targetHouse = await _db.Houses.FirstOrDefaultAsync(h => h.Id == id);

            if (targetHouse == null)
            {
                return BadRequest(); // Връщаме грешка 400 Bad Request според условието
            }

            // Правим мапване към ViewModel, както се изисква в Упражнение 3
            var houseDetailsModel = new Models.House.HouseDetailsViewModel
            {
                Id = targetHouse.Id,
                Title = targetHouse.Title,
                Address = targetHouse.Address,
                Description = targetHouse.Description,
                ImageUrl = targetHouse.ImageUrl,
                PricePerMonth = targetHouse.PricePerMonth
                // Ако имаш Category и Agent, ги добавяш тук
            };

            return View(houseDetailsModel);
        }

        [HttpGet]
        public IActionResult CreateHouse()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateHouse(HouseFormViewModel formModel)
        {
            if (!ModelState.IsValid)
            {
                return View(formModel);
            }

            // Леко променена логика за проверка на адреса
            bool isAddressTaken = await _db.Houses
                .AnyAsync(prop => prop.Address.ToLower() == formModel.Address.ToLower());

            if (isAddressTaken)
            {
                ModelState.AddModelError(nameof(formModel.Address), "This address is already registered in the system.");
                return View(formModel);
            }

            var houseToInsert = new House
            {
                Title = formModel.Title,
                Address = formModel.Address,
                Description = formModel.Description,
                ImageUrl = formModel.ImageUrl,
                PricePerMonth = formModel.PricePerMonth
            };

            _db.Houses.Add(houseToInsert);
            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(AllHouses));
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> MyHouses()
        {
            // В момента връщаме празен списък, докато не вържем логиката за конкретния потребител
            var userProperties = new List<Models.HousesViewModel>();
            return View(userProperties);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Edit(int id)
        {
            var houseToEdit = await _db.Houses.FindAsync(id);
            if (houseToEdit == null)
            {
                return BadRequest();
            }

            var model = new HouseFormViewModel
            {
                Title = houseToEdit.Title,
                Address = houseToEdit.Address,
                Description = houseToEdit.Description,
                ImageUrl = houseToEdit.ImageUrl,
                PricePerMonth = houseToEdit.PricePerMonth
            };

            return View(model);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Edit(int id, HouseFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var houseToUpdate = await _db.Houses.FindAsync(id);
            if (houseToUpdate == null)
            {
                return BadRequest();
            }

            houseToUpdate.Title = model.Title;
            houseToUpdate.Address = model.Address;
            houseToUpdate.Description = model.Description;
            houseToUpdate.ImageUrl = model.ImageUrl;
            houseToUpdate.PricePerMonth = model.PricePerMonth;

            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Details), new { id = houseToUpdate.Id });
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            var houseToDelete = await _db.Houses.FindAsync(id);
            if (houseToDelete == null)
            {
                return BadRequest();
            }

            var model = new Models.House.HouseDetailsViewModel
            {
                Id = houseToDelete.Id,
                Title = houseToDelete.Title,
                Address = houseToDelete.Address,
                ImageUrl = houseToDelete.ImageUrl
            };

            return View(model);
        }

        [HttpPost, ActionName("Delete")]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var house = await _db.Houses.FindAsync(id);
            if (house != null)
            {
                _db.Houses.Remove(house);
                await _db.SaveChangesAsync();
            }

            return RedirectToAction(nameof(AllHouses));
        }

        [HttpPost]
        [Authorize]
        public IActionResult Rent(int id)
        {
            // Логика за наемане
            return RedirectToAction(nameof(MyHouses));
        }

        [HttpPost]
        [Authorize]
        public IActionResult Leave(int id)
        {
            // Логика за напускане на къщата
            return RedirectToAction(nameof(MyHouses));
        }
    }
}