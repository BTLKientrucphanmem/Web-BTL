using Web_BTL.BusinessLogicLayer.Services;
using Web_BTL.DataAccessLayer.Models;

namespace Web_BTL.DataAccessLayer.Repository {
    // Lớp triển khai logic nghiệp vụ liên quan đến người dùng
    public class UserService : IUserService {
        private readonly IUserRepository _userRepository; // Repository truy cập dữ liệu

        // Constructor: Tiêm IUserRepository qua Dependency Injection
        public UserService(IUserRepository userRepository) {
            _userRepository = userRepository;
        }

        // Lấy thông tin người dùng (admin hoặc customer) dựa trên email
        public async Task<(object? User, bool IsAdmin)> GetUserInformationAsync(string? email) {
            if (string.IsNullOrEmpty(email))
                return (null, false); // Không có email, trả về null

            // Kiểm tra xem người dùng là admin
            var admin = await _userRepository.GetAdminByEmailAsync(email);
            if (admin != null)
                return (admin, true); // Trả về admin và xác nhận là admin

            // Nếu không phải admin, kiểm tra khách hàng
            var customer = await _userRepository.GetCustomerByEmailAsync(email);
            return (customer, false); // Trả về khách hàng và xác nhận không phải admin
        }

        // Lấy thông tin người dùng để hiển thị form chỉnh sửa hình ảnh
        public async Task<(object? User, bool IsAdmin)> GetUserForImageAsync(string? email) {
            // Gọi phương thức chung để lấy thông tin người dùng
            return await GetUserInformationAsync(email);
        }

        // Lưu hình ảnh người dùng và cập nhật đường dẫn
        public async Task SaveUserImageAsync(string? email, IFormFile image, IWebHostEnvironment environment, SaveImageVideo saveService) {
            if (string.IsNullOrEmpty(email) || image == null || image.Length <= 0)
                return; // Không có email hoặc hình ảnh không hợp lệ, thoát hàm

            // Kiểm tra admin
            var admin = await _userRepository.GetAdminByEmailAsync(email);
            if (admin != null) {
                // Lưu hình ảnh và cập nhật đường dẫn
                admin.UserImagePath = await saveService.SaveImageAsync(environment, "images/users", admin.UserImagePath, admin.UserLogin, image);
                await _userRepository.UpdateAdminAsync(admin);
                return;
            }

            // Kiểm tra khách hàng
            var customer = await _userRepository.GetCustomerByEmailAsync(email);
            if (customer != null) {
                // Lưu hình ảnh và cập nhật đường dẫn
                customer.UserImagePath = await saveService.SaveImageAsync(environment, "images/users", customer.UserImagePath, customer.UserLogin, image);
                await _userRepository.UpdateCustomerAsync(customer);
            }
        }

        // Lấy thông tin người dùng để hiển thị form chỉnh sửa tên
        public async Task<(object? User, bool IsAdmin)> GetUserForNameAsync(string? email) {
            // Gọi phương thức chung để lấy thông tin người dùng
            return await GetUserInformationAsync(email);
        }

        // Cập nhật tên người dùng
        public async Task UpdateUserNameAsync(string? email, string name) {
            if (string.IsNullOrEmpty(email))
                return; // Không có email, thoát hàm

            // Kiểm tra admin
            var admin = await _userRepository.GetAdminByEmailAsync(email);
            if (admin != null) {
                admin.UserName = name;
                await _userRepository.UpdateAdminAsync(admin);
                return;
            }

            // Kiểm tra khách hàng
            var customer = await _userRepository.GetCustomerByEmailAsync(email);
            if (customer != null) {
                customer.UserName = name;
                await _userRepository.UpdateCustomerAsync(customer);
            }
        }

        // Lấy thông tin người dùng để hiển thị form chỉnh sửa mật khẩu
        public async Task<(object? User, bool IsAdmin)> GetUserForPasswordAsync(string? email) {
            // Gọi phương thức chung để lấy thông tin người dùng
            return await GetUserInformationAsync(email);
        }

        // Cập nhật mật khẩu người dùng với kiểm tra hợp lệ
        public async Task<(bool Success, string? ErrorMessage)> UpdateUserPasswordAsync(string? email, string oldPassword, string newPassword, string confirmPassword) {
            if (string.IsNullOrEmpty(email))
                return (false, "Không tìm thấy email người dùng");

            // Kiểm tra admin
            var admin = await _userRepository.GetAdminByEmailAsync(email);
            if (admin != null) {
                var (success, error) = CheckPassword(admin, oldPassword, newPassword, confirmPassword);
                if (!success)
                    return (false, error);

                admin.LoginPassword = newPassword;
                await _userRepository.UpdateAdminAsync(admin);
                return (true, null);
            }

            // Kiểm tra khách hàng
            var customer = await _userRepository.GetCustomerByEmailAsync(email);
            if (customer != null) {
                var (success, error) = CheckPassword(customer, oldPassword, newPassword, confirmPassword);
                if (!success)
                    return (false, error);

                customer.LoginPassword = newPassword;
                await _userRepository.UpdateCustomerAsync(customer);
                return (true, null);
            }

            return (false, "Không tìm thấy người dùng");
        }

        // Phương thức riêng kiểm tra mật khẩu
        private (bool Success, string? ErrorMessage) CheckPassword(UserModel user, string oldPassword, string newPassword, string confirmPassword) {
            if (oldPassword != user.LoginPassword)
                return (false, "Mật khẩu cũ không đúng");
            if (oldPassword == newPassword)
                return (false, "Không được dùng lại mật khẩu cũ");
            if (newPassword != confirmPassword)
                return (false, "Mật khẩu điền lại không đúng");
            return (true, null);
        }
    }
}