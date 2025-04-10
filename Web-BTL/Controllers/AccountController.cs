using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web_BTL.Models;
using Web_BTL.Repository;
using Microsoft.Extensions.Caching.Memory;

namespace Web_BTL.Controllers
{
    public class AccountController : Controller
    {
        private readonly DBXemPhimContext _dataContext;

        public AccountController(DBXemPhimContext dataContext)
        {
            _dataContext = dataContext;
        }

        [HttpGet]
        public IActionResult SignIn()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignIn(LogInModel model)
        {
            if (ModelState.IsValid)
            {
                var admin = await _dataContext.Admins
                    .FirstOrDefaultAsync(a =>
                        (a.UserEmail == model.LogInName || a.UserLogin == model.LogInName) &&
                        a.LoginPassword == model.Password &&
                        a.UserState == true);

                if (admin == null)
                {
                    var customer = await _dataContext.Customers
                        .FirstOrDefaultAsync(c =>
                            (c.UserEmail == model.LogInName || c.UserLogin == model.LogInName) &&
                            c.LoginPassword == model.Password &&
                            c.UserState == true);

                    if (customer != null)
                    {
                        HttpContext.Session.SetString("LogIn Session", customer.UserEmail);
                        return RedirectToAction(nameof(Index), "Home");
                    }

                    return View(model);
                }

                HttpContext.Session.SetString("LogIn Session", admin.UserEmail);
                HttpContext.Session.SetString("Admin", admin.Role.ToString());
                return RedirectToAction(nameof(Index), "Home");
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(CustomerModel model)
        {
            if (ModelState.IsValid)
            {
                var admin = await _dataContext.Admins
                    .FirstOrDefaultAsync(a => a.UserEmail == model.UserEmail || a.UserLogin == model.UserLogin);
                var customer = await _dataContext.Customers
                    .FirstOrDefaultAsync(c => c.UserEmail == model.UserEmail || c.UserLogin == model.UserLogin);

                if (customer == null && admin == null)
                {
                    model.UserImagePath = "default.jpg";
                    model.UserState = true;
                    model.UserCreateDate = DateTime.Now;

                    _dataContext.Customers.Add(model);
                    await _dataContext.SaveChangesAsync();

                    HttpContext.Session.SetString("LogIn Session", model.UserEmail);
                    return RedirectToAction(nameof(Index), "Home");
                }

                ModelState.AddModelError(string.Empty, "Email hoặc tên đăng nhập đã tồn tại");
                return View(model);
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult RecoverPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RecoverPassword(LogInModel model)
        {
            if (ModelState.IsValid)
            {
                var cus = await _dataContext.Customers
                    .FirstOrDefaultAsync(c =>
                        c.UserLogin == model.LogInName || c.UserEmail == model.LogInName);

                if (cus != null)
                {
                    cus.LoginPassword = model.Password;
                    await _dataContext.SaveChangesAsync();
                    Console.WriteLine("Cập nhật mật khẩu thành công.");
                    return RedirectToAction(nameof(SignIn));
                }

                Console.WriteLine("Không tìm thấy người dùng.");
                return View(model);
            }

            Console.WriteLine("Thông tin không hợp lệ.");
            return View(model);
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
