using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web_BTL.BusinessLogicLayer.Services;
using Web_BTL.DataAccessLayer;
using Web_BTL.DataAccessLayer.Models;


namespace Web_BTL.BusinessLogicLayer.Controllers {
    public class UserController : Controller
    {
        private readonly DBXemPhimContext _dataContext;
        private readonly IWebHostEnvironment _environment;
        private readonly SaveImageVideo _save;
        public UserController(DBXemPhimContext dataContext, IWebHostEnvironment environment, SaveImageVideo save)
        {
            _dataContext = dataContext;
            _environment = environment;
            _save = save;
        }
        [HttpGet]
        public async Task<IActionResult> UserInformation()
        {
            // lấy thông tin người dùng từ s
            string email = HttpContext.Session.GetString("LogIn Session");
            string role = HttpContext.Session.GetString("Admin");
            if (email != null)
            {
                if (role != null)
                {
                    var admin = await _dataContext.Admins.FirstOrDefaultAsync(c => c.UserEmail == email);
                    if (admin != null) return View(admin);
                }
                var customer = await _dataContext.Customers.FirstOrDefaultAsync(c => c.UserEmail == email);
                if (customer != null) return View(customer);
                else
                {
                    Console.WriteLine("Khong co du lieu nguoi dung");
                    return RedirectToAction(nameof(SignIn), "Account");
                }
            }
            Console.WriteLine("Khong co email");
            return RedirectToAction(nameof(SignIn), "Account");
        }
        public async Task<IActionResult> Image()
        {
            string email = HttpContext.Session.GetString("LogIn Session");
            var admin = await _dataContext.Admins.FirstOrDefaultAsync(c => c.UserEmail == email);
            if (admin != null) return PartialView("SaveImage", admin);
            var customer = await _dataContext.
                Customers.
                FirstOrDefaultAsync(c => c.UserEmail == email);
            return PartialView("SaveImage", customer);
        }
        [HttpPost]
        public async Task<IActionResult> SaveImage(IFormFile image)
        {
            if (image != null && image.Length > 0)
            {
                Console.WriteLine("Da lay duoc anh vao post SaveImage");
                string email = HttpContext.Session.GetString("LogIn Session");
                string role = HttpContext.Session.GetString("Admin");
                var admin = await _dataContext.Admins.FirstOrDefaultAsync(a => a.UserEmail == email);
                if (admin != null) admin.UserImagePath = await _save.SaveImageAsync(_environment, "images/users", admin.UserImagePath, admin.UserLogin, image);
                else
                {
                    var customer = await _dataContext.Customers.FirstOrDefaultAsync(c => c.UserEmail == email);
                    if (customer != null) customer.UserImagePath = await _save.SaveImageAsync(_environment, "images/users", customer.UserImagePath, customer.UserLogin, image);
                }
                await _dataContext.SaveChangesAsync();
            }
            else
                Console.WriteLine("Khong lay duoc anh ve");
            return RedirectToAction(nameof(UserInformation));
        }
        public async Task<IActionResult> Name()
        {
            string email = HttpContext.Session.GetString("LogIn Session");
            var admin = await _dataContext.Admins.FirstOrDefaultAsync(c => c.UserEmail == email);
            if (admin != null) return PartialView("EditName", admin);
            var customer = await _dataContext.Customers.FirstOrDefaultAsync(c => c.UserEmail == email);
            return PartialView("EditName", customer);
        }
        [HttpPost]
        public async Task<IActionResult> EditName(string name)
        {
            string email = HttpContext.Session.GetString("LogIn Session");
            var admin = await _dataContext.Admins.FirstOrDefaultAsync(c => c.UserEmail == email);
            if (admin != null) admin.UserName = name;
            else
            {
                var customer = await _dataContext.Customers.FirstOrDefaultAsync(c => c.UserEmail == email);
                customer.UserName = name;
            }
            await _dataContext.SaveChangesAsync();
            return RedirectToAction(nameof(UserInformation));
        }
        public async Task<IActionResult> rPassword()
        {
            string email = HttpContext.Session.GetString("LogIn Session");
            var admin = await _dataContext.Admins.FirstOrDefaultAsync(c => c.UserEmail == email);
            if (admin != null) return PartialView("EditPassword", admin);
            var customer = await _dataContext.Customers.FirstOrDefaultAsync(c => c.UserEmail == email);
            return PartialView("EditPassword", customer);
        }
        [HttpPost]
        public async Task<IActionResult> EditPassword(string oPassword, string nPassword, string cPassword)
        {
            string email = HttpContext.Session.GetString("LogIn Session");
            var admin = await _dataContext.Admins.FirstOrDefaultAsync(c => c.UserEmail == email);
            var customer = await _dataContext.Customers.FirstOrDefaultAsync(c => c.UserEmail == HttpContext.Session.GetString("LogIn Session"));
            if (admin != null) CheckPassword(admin, oPassword, nPassword, cPassword);
            else CheckPassword(customer, oPassword, nPassword, cPassword);
            if (!ModelState.IsValid)
                return View("UserInformation", customer);
            else
            {
                if (admin != null) admin.LoginPassword = nPassword;
                else customer.LoginPassword = nPassword;
                await _dataContext.SaveChangesAsync();
            }
            return RedirectToAction(nameof(UserInformation));
        }
        private void CheckPassword(UserModel model, string oPassword, string nPassword, string cPassword)
        {
            if (oPassword != model.LoginPassword)
                ModelState.AddModelError("ErrorPassword", "Mật khẩu cũ không đúng");
            else if (oPassword == nPassword)
                ModelState.AddModelError("ErrorPassword", "Không được dùng lại mật khẩu cũ");
            else if (nPassword != cPassword)
                ModelState.AddModelError("ErrorPassword", "Mật khẩu điền lại không đúng");
        }
        [HttpGet]
        public IActionResult LogOut()
        {
            string email = HttpContext.Session.GetString("LogIn Session");
            string role = HttpContext.Session.GetString("Admin");
            if (email != null)
                HttpContext.Session.Remove("LogIn Session");
            if (role != null)
                HttpContext.Session.Remove("Admin");
            return RedirectToAction(nameof(SignIn), "Account");
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
