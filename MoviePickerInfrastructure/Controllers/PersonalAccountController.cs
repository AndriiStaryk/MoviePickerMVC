using Microsoft.AspNetCore.Mvc;

namespace MoviePickerInfrastructure.Controllers
{
    public class PersonalAccountController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
