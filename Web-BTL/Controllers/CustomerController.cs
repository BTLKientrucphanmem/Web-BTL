using Microsoft.AspNetCore.Mvc;
using Web_BTL.BusinessLogicLayer.Services;

namespace Web_BTL.BusinessLogicLayer.Controllers {
    public class CustomerController : Controller {
        private readonly ICustomerService _customerService;

        // Constructor: Tiêm ICustomerService qua Dependency Injection
        public CustomerController(ICustomerService customerService) {
            _customerService = customerService;
        }

        // Hiển thị danh sách media yêu thích
        public async Task<IActionResult> Favourite() {
            // Gọi tầng nghiệp vụ để lấy danh sách media yêu thích
            var (success, favoriteMedias) = await _customerService.GetFavoriteMediasAsync();
            if (!success)
                return NotFound(); // Trả về lỗi nếu không hợp lệ (chưa đăng nhập, không phải khách hàng)

            return View(favoriteMedias); // Trả về View với danh sách media yêu thích
        }

        // Hiển thị danh sách media đã xem
        public async Task<IActionResult> Watched() {
            // Gọi tầng nghiệp vụ để lấy danh sách media đã xem
            var (success, watchedMedias) = await _customerService.GetWatchedMediasAsync();
            if (!success)
                return NotFound(); // Trả về lỗi nếu không hợp lệ (chưa đăng nhập, không phải khách hàng)

            return View(watchedMedias); // Trả về View với danh sách media đã xem
        }

        // Hiển thị trang chính của khách hàng
        public IActionResult Index() {
            return View(); // Trả về View mặc định
        }
    }
}