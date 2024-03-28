using Microsoft.AspNetCore.Mvc;

namespace ScopeIndiaWebsite.Controllers
{
    public class AboutUsController : Controller
    {
        public IActionResult AboutUs()
        {
            return View();
        }
    }
}
