

using Web_BTL.DataAccessLayer.Models;
using Web_BTL.DataAccessLayer.Repository;

namespace Web_BTL.BusinessLogicLayer.Services {
    public class AccountService : IAccountService {
        private readonly IAccountRepository _accountRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AccountService(IAccountRepository accountRepository, IHttpContextAccessor httpContextAccessor) {
            _accountRepository = accountRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<(bool Success, string Email, bool IsAdmin, string Role)> SignInAsync(string loginName, string password) {
            var admin = await _accountRepository.GetAdminByCredentialsAsync(loginName, password);
            if (admin != null) {
                _httpContextAccessor.HttpContext.Session.SetString("LogIn Session", admin.UserEmail);
                _httpContextAccessor.HttpContext.Session.SetString("Admin", admin.Role.ToString());
                return (true, admin.UserEmail, true, admin.Role.ToString());
            }

            var customer = await _accountRepository.GetCustomerByCredentialsAsync(loginName, password);
            if (customer != null) {
                _httpContextAccessor.HttpContext.Session.SetString("LogIn Session", customer.UserEmail);
                return (true, customer.UserEmail, false, null);
            }

            return (false, null, false, null);
        }

        public async Task<(bool Success, string ErrorMessage)> SignUpAsync(CustomerModel customer) {
            var userExists = await _accountRepository.IsUserExistsAsync(customer.UserEmail, customer.UserLogin);
            if (userExists) {
                return (false, "Email hoặc tên đăng nhập đã tồn tại");
            }

            customer.UserImagePath = "default.jpg";
            customer.UserState = true;
            customer.UserCreateDate = DateTime.Now;

            await _accountRepository.AddCustomerAsync(customer);
            _httpContextAccessor.HttpContext.Session.SetString("LogIn Session", customer.UserEmail);
            return (true, null);
        }

        public async Task<(bool Success, string ErrorMessage)> RecoverPasswordAsync(string loginName, string newPassword) {
            var customer = await _accountRepository.GetCustomerByLoginNameAsync(loginName);
            if (customer == null) {
                return (false, "Không tìm thấy người dùng.");
            }

            customer.LoginPassword = newPassword;
            await _accountRepository.UpdateCustomerAsync(customer);
            return (true, null);
        }
    }
}