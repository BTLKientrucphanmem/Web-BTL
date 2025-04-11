using Microsoft.AspNetCore.Mvc;
using Web_BTL.BusinessLogicLayer.Services;
using Web_BTL.DataAccessLayer.Models;

namespace Web_BTL.BusinessLogicLayer.Controllers {
    public class AccountController : Controller {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService) {
            _accountService = accountService;
        }

        [HttpGet]
        public IActionResult SignIn() {
            return View(new LogInModel());
        }

        [HttpPost]
        public async Task<IActionResult> SignIn(LogInModel model) {
            if (!ModelState.IsValid) {
                return View(model);
            }

            var (success, email, isAdmin, role) = await _accountService.SignInAsync(model.LogInName, model.Password);
            if (success) {
                return RedirectToAction(nameof(Index), "Home");
            }

            ModelState.AddModelError(string.Empty, "Thông tin đăng nhập không hợp lệ");
            return View(model);
        }

        [HttpGet]
        public IActionResult SignUp() {
            return View(new CustomerModel());
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(CustomerModel model) {
            if (!ModelState.IsValid) {
                return View(model);
            }

            var (success, errorMessage) = await _accountService.SignUpAsync(model);
            if (success) {
                return RedirectToAction(nameof(Index), "Home");
            }

            ModelState.AddModelError(string.Empty, errorMessage);
            return View(model);
        }

        [HttpGet]
        public IActionResult RecoverPassword() {
            return View(new LogInModel());
        }

        [HttpPost]
        public async Task<IActionResult> RecoverPassword(LogInModel model) {
            if (!ModelState.IsValid) {
                return View(model);
            }

            var (success, errorMessage) = await _accountService.RecoverPasswordAsync(model.LogInName, model.Password);
            if (success) {
                return RedirectToAction(nameof(SignIn));
            }

            ModelState.AddModelError(string.Empty, errorMessage);
            return View(model);
        }

        public IActionResult Index() {
            return View();
        }
    }
}