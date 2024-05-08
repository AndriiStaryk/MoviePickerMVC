using Microsoft.AspNetCore.Mvc;

namespace MoviePickerInfrastructure.Controllers;

public class DB_LabController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
    public IActionResult Request1()
    {
        return View();
    }
}
