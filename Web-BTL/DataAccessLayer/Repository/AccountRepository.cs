using Microsoft.EntityFrameworkCore;
using Web_BTL.DataAccessLayer.Models;

namespace Web_BTL.DataAccessLayer.Repository {
    public class AccountRepository : IAccountRepository {
        private readonly DBXemPhimContext _dataContext;

        public AccountRepository(DBXemPhimContext dataContext) {
            _dataContext = dataContext;
        }

        public async Task<AdminModel> GetAdminByCredentialsAsync(string loginName, string password) {
            return await _dataContext.Admins
                .FirstOrDefaultAsync(a =>
                    (a.UserEmail == loginName || a.UserLogin == loginName) &&
                    a.LoginPassword == password &&
                    a.UserState == true);
        }

        public async Task<CustomerModel> GetCustomerByCredentialsAsync(string loginName, string password) {
            return await _dataContext.Customers
                .FirstOrDefaultAsync(c =>
                    (c.UserEmail == loginName || c.UserLogin == loginName) &&
                    c.LoginPassword == password &&
                    c.UserState == true);
        }

        public async Task<bool> IsUserExistsAsync(string email, string loginName) {
            var adminExists = await _dataContext.Admins
                .AnyAsync(a => a.UserEmail == email || a.UserLogin == loginName);
            var customerExists = await _dataContext.Customers
                .AnyAsync(c => c.UserEmail == email || c.UserLogin == loginName);
            return adminExists || customerExists;
        }

        public async Task AddCustomerAsync(CustomerModel customer) {
            _dataContext.Customers.Add(customer);
            await _dataContext.SaveChangesAsync();
        }

        public async Task<CustomerModel> GetCustomerByLoginNameAsync(string loginName) {
            return await _dataContext.Customers
                .FirstOrDefaultAsync(c => c.UserLogin == loginName || c.UserEmail == loginName);
        }

        public async Task UpdateCustomerAsync(CustomerModel customer) {
            _dataContext.Customers.Update(customer);
            await _dataContext.SaveChangesAsync();
        }
    }
}