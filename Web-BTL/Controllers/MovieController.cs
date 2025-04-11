using Microsoft.AspNetCore.Mvc;
using Web_BTL.BusinessLogicLayer.Services;

namespace Web_BTL.Controllers {
    // Controller xử lý các yêu cầu liên quan đến phim
    public class MovieController : Controller {
        private readonly IMovieService _movieService; // Dịch vụ nghiệp vụ xử lý logic liên quan đến phim

        // Constructor: Tiêm IMovieService qua Dependency Injection
        public MovieController(IMovieService movieService) {
            _movieService = movieService;
        }

        // Hiển thị thông tin phim và thêm vào danh sách xem nếu chưa có
        public async Task<IActionResult> Index(int movieId) {
            // Lấy email người dùng từ Session
            var userEmail = HttpContext.Session.GetString("LogIn Session");

            // Gọi tầng nghiệp vụ để lấy thông tin phim và cập nhật danh sách xem
            var movie = await _movieService.GetMediaAndUpdateWatchListAsync(movieId, userEmail);

            // Nếu không tìm thấy phim, trả về lỗi 404
            if (movie == null)
                return NotFound();

            // Trả về View với thông tin phim
            return View(movie);
        }

        // Hiển thị trang xem phim và cập nhật trạng thái đã xem
        public async Task<IActionResult> Watching(int movieId) {
            // Lấy email người dùng từ Session
            var userEmail = HttpContext.Session.GetString("LogIn Session");

            // Gọi tầng nghiệp vụ để lấy thông tin phim và đánh dấu đã xem
            var movie = await _movieService.GetMediaAndMarkAsWatchedAsync(movieId, userEmail);

            // Nếu không tìm thấy phim, trả về lỗi 404
            if (movie == null)
                return NotFound();

            // Trả về View với thông tin phim
            return View(movie);
        }

        // Thêm phim vào danh sách yêu thích (POST)
        [HttpPost]
        public async Task<IActionResult> AddFavourite(int mid) {
            // Lấy email người dùng từ Session
            var userEmail = HttpContext.Session.GetString("LogIn Session");

            // Gọi tầng nghiệp vụ để thêm phim vào danh sách yêu thích
            var (success, isRedirectToLogin) = await _movieService.AddFavouriteAsync(mid, userEmail);

            // Nếu cần chuyển hướng đến trang đăng nhập, trả về PartialView tương ứng
            if (isRedirectToLogin)
                return PartialView("redirect-login");

            // Nếu không tìm thấy bản ghi hoặc thất bại, trả về lỗi 404
            if (!success)
                return NotFound();

            // Trả về PartialView xác nhận thêm yêu thích thành công
            return PartialView("AddFavourite");
        }
    }
}