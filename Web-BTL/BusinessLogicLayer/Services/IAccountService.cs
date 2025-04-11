using Web_BTL.DataAccessLayer.Models;
namespace Web_BTL.BusinessLogicLayer.Services {
    public interface IAccountService {
        Task<(bool Success, string Email, bool IsAdmin, string Role)> SignInAsync(string loginName, string password);
        Task<(bool Success, string ErrorMessage)> SignUpAsync(CustomerModel customer);
        Task<(bool Success, string ErrorMessage)> RecoverPasswordAsync(string loginName, string newPassword);
    }
}

