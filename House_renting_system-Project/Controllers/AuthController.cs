using House_renting_system_Project.Models.Auth;
using Microsoft.AspNetCore.Mvc;

namespace House_renting_system_Project.Controllers
{
    public class AuthController : Controller
    {
        public IActionResult Login()
        {
            return View(); 
        }
        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            return View();
        }
    }
}
