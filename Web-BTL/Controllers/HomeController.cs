using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Web_BTL.BusinessLogicLayer.Services;
using Web_BTL.DataAccessLayer.Models;

namespace Web_BTL.Controllers {
    // Controller xử lý các yêu cầu cho trang chủ
    public class HomeController : Controller {
        private readonly IHomeService _homeService; // Dịch vụ nghiệp vụ xử lý logic trang chủ

        // Constructor: Tiêm IHomeService qua Dependency Injection
        public HomeController(IHomeService homeService) {
            _homeService = homeService;
        }

        // Hiển thị trang chủ với danh sách tất cả media và các danh sách theo loại
        public async Task<IActionResult> Index() {
            // Gọi tầng nghiệp vụ để lấy dữ liệu cho trang chủ
            var (allMedias, cartoons, movies, series) = await _homeService.GetHomeDataAsync();

            // Gán dữ liệu vào ViewBag để hiển thị trong View
            ViewBag.Cartoon = cartoons; // Danh sách media hoạt hình
            ViewBag.Movie = movies;     // Danh sách phim lẻ
            ViewBag.Series = series;    // Danh sách phim bộ

            // Trả về View với danh sách tất cả media
            return View(allMedias);
        }

        // Hiển thị trang chính sách bảo mật
        public IActionResult Privacy() {
            // Trả về View cho trang Privacy
            return View();
        }

        // Xử lý lỗi và hiển thị trang lỗi (không lưu cache)
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() {
            // Tạo model lỗi với RequestId để hiển thị thông tin lỗi
            var errorModel = new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier };

            // Trả về View lỗi với model
            return View(errorModel);
        }

        // Lọc và hiển thị danh sách media theo ID thể loại
        public async Task<IActionResult> GenreFilter(int id) {
            // Gọi tầng nghiệp vụ để lấy danh sách media theo ID thể loại
            var movies = await _homeService.GetMediasByGenreIdAsync(id);

            // Trả về View với danh sách media đã lọc
            return View(movies);
        }

        // Lọc và hiển thị danh sách media hoạt hình
        public async Task<IActionResult> CartoonMovieFilter() {
            // Gọi tầng nghiệp vụ để lấy danh sách media hoạt hình
            var movies = await _homeService.GetCartoonMediasAsync();

            // Gán dữ liệu vào ViewBag để hiển thị trong View
            ViewBag.Cartoon = movies;

            // Trả về View cho danh sách media hoạt hình
            return View();
        }
    }
}