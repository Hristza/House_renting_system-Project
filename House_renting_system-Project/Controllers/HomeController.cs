using House_renting_system_Project.Data.Data;
using House_renting_system_Project.Servises;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace House_renting_system_Project.Controllers
{
    public class HomeController : Controller
    {
        private readonly HouseRentingDbContext context;

        public HomeController(HouseRentingDbContext context)
        {
            this.context = context;
        }

        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var model = new HomeViewModel
            {
                IsAuthenticated = User.Identity.IsAuthenticated
            };

            if (User.Identity.IsAuthenticated && !string.IsNullOrEmpty(userId))
            {
                model.UserHousesCount = await context.Houses
                    .CountAsync(h => h.AgentId == userId && h.IsDeleted == false);
            }

            return View(model);
        }

        [Route("Home/Error")]
        public IActionResult Error(int? statusCode)
        {
            if (statusCode.HasValue)
            {
                switch (statusCode.Value)
                {
                    case 401:
                        return View("Error401");
                    case 404:
                        return View("Error404");
                }
            }
            return View("Error404");
        }

        public IActionResult ServerError()
        {
            return View("Error500");
        }
    }
}