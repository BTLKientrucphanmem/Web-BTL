
using Web_BTL.BusinessLogicLayer.Services;
using Web_BTL.DataAccessLayer.Models;
using Web_BTL.DataAccessLayer.Repository;

namespace WWeb_BTL.BusinessLogicLayer.Services {
    public class CustomerService : ICustomerService {
        private readonly ICustomerRepository _customerRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CustomerService(ICustomerRepository customerRepository, IHttpContextAccessor httpContextAccessor) {
            _customerRepository = customerRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        // Lấy danh sách media yêu thích
        public async Task<(bool Success, List<MediaModel> FavoriteMedias)> GetFavoriteMediasAsync() {
            // Kiểm tra xem có phải khách hàng hợp lệ không
            if (!ValidateCustomer())
                return (false, new List<MediaModel>());

            // Lấy email từ Session
            string email = _httpContextAccessor.HttpContext?.Session.GetString("LogIn Session");
            if (string.IsNullOrEmpty(email))
                return (false, new List<MediaModel>());

            // Lấy thông tin khách hàng từ tầng dữ liệu
            var customer = await _customerRepository.GetCustomerByEmailAsync(email);
            if (customer == null)
                return (false, new List<MediaModel>());

            // Lấy danh sách media yêu thích từ tầng dữ liệu
            var favoriteMedias = await _customerRepository.GetFavoriteMediasAsync(customer.CustomerId);
            return (true, favoriteMedias);
        }

        // Lấy danh sách media đã xem
        public async Task<(bool Success, List<MediaModel> WatchedMedias)> GetWatchedMediasAsync() {
            // Kiểm tra xem có phải khách hàng hợp lệ không
            if (!ValidateCustomer())
                return (false, new List<MediaModel>());

            // Lấy email từ Session
            string email = _httpContextAccessor.HttpContext?.Session.GetString("LogIn Session");
            if (string.IsNullOrEmpty(email))
                return (false, new List<MediaModel>());

            // Lấy thông tin khách hàng từ tầng dữ liệu
            var customer = await _customerRepository.GetCustomerByEmailAsync(email);
            if (customer == null)
                return (false, new List<MediaModel>());

            // Lấy danh sách media đã xem từ tầng dữ liệu
            var watchedMedias = await _customerRepository.GetWatchedMediasAsync(customer.CustomerId);
            return (true, watchedMedias);
        }

        // Xác thực xem người dùng có phải là khách hàng không (không phải admin)
        public bool ValidateCustomer() {
            // Nếu có Session "Admin" thì đây không phải khách hàng
            return _httpContextAccessor.HttpContext?.Session.GetString("Admin") == null;
        }
    }
}