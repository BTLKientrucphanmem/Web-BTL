


namespace Web_BTL.BusinessLogicLayer.Services {
    // Giao diện định nghĩa các phương thức nghiệp vụ liên quan đến người dùng
    public interface IUserService {
        // Lấy thông tin người dùng (admin hoặc customer) dựa trên email
        Task<(object? User, bool IsAdmin)> GetUserInformationAsync(string? email);

        // Lấy thông tin người dùng để hiển thị form chỉnh sửa hình ảnh
        Task<(object? User, bool IsAdmin)> GetUserForImageAsync(string? email);

        // Lưu hình ảnh người dùng và cập nhật đường dẫn
        Task SaveUserImageAsync(string? email, IFormFile image, IWebHostEnvironment environment, SaveImageVideo saveService);

        // Lấy thông tin người dùng để hiển thị form chỉnh sửa tên
        Task<(object? User, bool IsAdmin)> GetUserForNameAsync(string? email);

        // Cập nhật tên người dùng
        Task UpdateUserNameAsync(string? email, string name);

        // Lấy thông tin người dùng để hiển thị form chỉnh sửa mật khẩu
        Task<(object? User, bool IsAdmin)> GetUserForPasswordAsync(string? email);

        // Cập nhật mật khẩu người dùng với kiểm tra hợp lệ
        Task<(bool Success, string? ErrorMessage)> UpdateUserPasswordAsync(string? email, string oldPassword, string newPassword, string confirmPassword);
    }
}