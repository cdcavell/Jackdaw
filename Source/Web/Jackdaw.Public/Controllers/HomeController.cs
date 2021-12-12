using Microsoft.AspNetCore.Mvc;

namespace Jackdaw.Public.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
