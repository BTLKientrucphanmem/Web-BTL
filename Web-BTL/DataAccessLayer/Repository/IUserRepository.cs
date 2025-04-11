using Web_BTL.DataAccessLayer.Models;

namespace Web_BTL.DataAccessLayer.Repository {
    // Giao diện định nghĩa các phương thức truy xuất và cập nhật dữ liệu liên quan đến người dùng
    public interface IUserRepository {
        // Lấy thông tin admin theo email
        Task<AdminModel?> GetAdminByEmailAsync(string email);

        // Lấy thông tin khách hàng theo email
        Task<CustomerModel?> GetCustomerByEmailAsync(string email);

        // Cập nhật thông tin admin
        Task UpdateAdminAsync(AdminModel admin);

        // Cập nhật thông tin khách hàng
        Task UpdateCustomerAsync(CustomerModel customer);
    }
}