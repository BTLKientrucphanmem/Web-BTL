using Web_BTL.DataAccessLayer.Models;

namespace Web_BTL.DataAccessLayer.Repository {
    public interface IAccountRepository {
        Task<AdminModel> GetAdminByCredentialsAsync(string loginName, string password);
        Task<CustomerModel> GetCustomerByCredentialsAsync(string loginName, string password);
        Task<bool> IsUserExistsAsync(string email, string loginName);
        Task AddCustomerAsync(CustomerModel customer);
        Task<CustomerModel> GetCustomerByLoginNameAsync(string loginName);
        Task UpdateCustomerAsync(CustomerModel customer);
    }
}
