using Microsoft.EntityFrameworkCore;
using Web_BTL.DataAccessLayer.Models;


namespace Web_BTL.DataAccessLayer.Repository {
    // Lớp triển khai truy xuất và cập nhật dữ liệu từ cơ sở dữ liệu cho người dùng
    public class UserRepository : IUserRepository {
        private readonly DBXemPhimContext _db; // DbContext để truy cập cơ sở dữ liệu

        // Constructor: Tiêm DBXemPhimContext qua Dependency Injection
        public UserRepository(DBXemPhimContext db) {
            _db = db;
        }

        // Lấy thông tin admin theo email
        public async Task<AdminModel?> GetAdminByEmailAsync(string email) {
            return await _db.Admins
                .FirstOrDefaultAsync(a => a.UserEmail == email); // Trả về admin khớp với email hoặc null
        }

        // Lấy thông tin khách hàng theo email
        public async Task<CustomerModel?> GetCustomerByEmailAsync(string email) {
            return await _db.Customers
                .FirstOrDefaultAsync(c => c.UserEmail == email); // Trả về khách hàng khớp với email hoặc null
        }

        // Cập nhật thông tin admin
        public async Task UpdateAdminAsync(AdminModel admin) {
            _db.Admins.Update(admin); // Cập nhật bản ghi admin trong cơ sở dữ liệu
            await _db.SaveChangesAsync(); // Lưu thay đổi
        }

        // Cập nhật thông tin khách hàng
        public async Task UpdateCustomerAsync(CustomerModel customer) {
            _db.Customers.Update(customer); // Cập nhật bản ghi khách hàng trong cơ sở dữ liệu
            await _db.SaveChangesAsync(); // Lưu thay đổi
        }
    }
}