using Microsoft.AspNetCore.Mvc;

namespace Mvc.Areas.Portal.Controllers
{
    [Area("Portal")]
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
