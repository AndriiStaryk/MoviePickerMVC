using Microsoft.AspNetCore.Mvc;

namespace MoviePickerInfrastructure.Controllers
{
    public class MainPageController : Controller
    {
        public IActionResult Index()
        {
            return View();
        } 

        public IActionResult ChangeLanguage(string culture)
        {
            // Set language preference in a cookie
            Response.Cookies.Append("language", culture, new CookieOptions { Path = "/" });

            // Redirect back to the previous page
            return Redirect(Request.Headers["Referer"].ToString());
        }
    }
}
