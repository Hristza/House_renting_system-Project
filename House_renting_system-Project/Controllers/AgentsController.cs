using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace House_renting_system_Project.Controllers
{
    [Authorize] // Само логнати потребители могат да стават агенти
    public class AgentsController : Controller
    {
        [HttpGet]
        public IActionResult Become()
        {
            // Тук в бъдеще ще добавим проверка дали потребителят вече не е агент
            return View();
        }

        [HttpPost]
        public IActionResult Become(string phoneNumber)
        {
            // Тук в бъдеще ще записваме новия агент в базата данни
            return RedirectToAction("Index", "Home");
        }
    }
}