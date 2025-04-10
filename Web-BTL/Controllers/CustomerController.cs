using Microsoft.AspNetCore.Mvc;
using Web_BTL.Repository;

namespace Web_BTL.Controllers
{
    public class CustomerController : Controller
    {
        private readonly DBXemPhimContext _dataContext;

        public CustomerController(DBXemPhimContext dataContext)
        {
            _dataContext = dataContext;
        }
        public IActionResult Favourite()
        {
            if (!validateCustomer()) return NotFound();
            string email = HttpContext.Session.GetString("LogIn Session");
            var customer = _dataContext.Customers.FirstOrDefault(c => c.UserEmail == email);
            if (customer == null) return RedirectToAction("SignIn", "Account");
            int watchListId = customer.CustomerId;

            // Truy vấn để lấy các Media có Favorite = true trong danh sách của khách hàng
            var favoriteList = (from lm in _dataContext.ListMedia
                                join m in _dataContext.Medias on lm.MediaId equals m.MediaId
                                where lm.WatchListId == watchListId && lm.Favorite == true
                                select m).ToList();

            return View(favoriteList);
        }
        public IActionResult Watched()
        {
            if (!validateCustomer()) return NotFound();
            string email = HttpContext.Session.GetString("LogIn Session");
            var customer = _dataContext.Customers.FirstOrDefault(c => c.UserEmail == email);
            if (customer == null) return RedirectToAction("SignIn", "Account");
            int watchListId = customer.CustomerId;

            // Truy vấn để lấy các Media có Favorite = true trong danh sách của khách hàng
            var watched = (from lm in _dataContext.ListMedia
                                join m in _dataContext.Medias on lm.MediaId equals m.MediaId
                                where lm.WatchListId == watchListId && lm.IsWatched == true
                                select m).ToList();
            return View(watched);
        }
        public IActionResult Index()
        {
            return View();
        }
        private bool validateCustomer()
        {
            if (HttpContext.Session.GetString("Admin") != null) return false;
            return true;
        }
    }
}