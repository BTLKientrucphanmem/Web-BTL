using Microsoft.AspNetCore.Mvc;

namespace Web_BTL.BusinessLogicLayer.Controllers
{
    public class AboutController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
