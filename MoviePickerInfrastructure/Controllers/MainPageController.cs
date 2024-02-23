using Microsoft.AspNetCore.Mvc;

namespace MoviePickerInfrastructure.Controllers
{
    public class MainPageController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Action1()
        {
            return View();
        }

        public IActionResult Action2()
        {
            return View();
        }

        public IActionResult Action3()
        {
            return View();
        }
    }
}
