using Microsoft.AspNetCore.Mvc;
using Web_BTL.DataAccessLayer.Services;


namespace Web_BTL.Controllers {
    // Controller xử lý các yêu cầu liên quan đến thể loại và danh sách media
    public class GenreController : Controller {
        private readonly IGenreService _genreService; // Dịch vụ nghiệp vụ xử lý logic liên quan đến thể loại và media

        // Constructor: Tiêm IGenreService qua Dependency Injection
        public GenreController(IGenreService genreService) {
            _genreService = genreService;
        }

        // Hiển thị danh sách media với các bộ lọc (diễn viên, thể loại, chất lượng, thời lượng)
        public async Task<IActionResult> Index(int? actorId, int? genreId, string quality, string duration) {
            // Gọi tầng nghiệp vụ để lấy danh sách media đã lọc và dữ liệu dropdown
            var (medias, actors, genres, qualities) = await _genreService.GetFilteredMediasAsync(actorId, genreId, quality, duration);

            // Gán dữ liệu vào ViewBag để hiển thị trong View
            ViewBag.AllActors = actors;   // Danh sách diễn viên cho dropdown
            ViewBag.AllGenres = genres;   // Danh sách thể loại cho dropdown
            ViewBag.AllQualities = qualities; // Danh sách chất lượng cho dropdown

            // Trả về View với danh sách media đã lọc
            return View(medias);
        }

        // Hiển thị danh sách tất cả media với phân trang và tìm kiếm
        public async Task<IActionResult> AllMedias(int? pageindex, string searchTerm) {
            // Gọi tầng nghiệp vụ để lấy danh sách media phân trang và thông tin phân trang
            var (medias, pageNumbers, currentPage, searchTermResult) = await _genreService.GetPagedMediasAsync(pageindex, searchTerm);

            // Gán dữ liệu vào ViewBag để hiển thị trong View
            ViewBag.PageNumbers = pageNumbers;   // Tổng số trang
            ViewBag.CurrentPage = currentPage;   // Trang hiện tại
            ViewBag.SearchTerm = searchTermResult; // Từ khóa tìm kiếm để giữ lại trong form

            // Trả về View với danh sách media phân trang
            return View(medias);
        }

        // Lọc media theo thể loại và trả về PartialView với số lượng giới hạn
        public async Task<IActionResult> MoviesFilter(int? mid) {
            // Gọi tầng nghiệp vụ để lấy danh sách media theo thể loại
            var medias = await _genreService.GetMediasByGenreIdAsync(mid);

            // Gán ID thể loại vào ViewBag để sử dụng trong PartialView nếu cần
            ViewBag.mid = mid;

            // Trả về PartialView với danh sách media
            return PartialView("_MediaPartial", medias);
        }

        // Lọc media theo thể loại và trả về PartialView với layout cụ thể
        public async Task<IActionResult> FilteredMedias(int genreId) {
            // Gọi tầng nghiệp vụ để lấy danh sách media theo thể loại
            var medias = await _genreService.GetFilteredMediasByGenreIdAsync(genreId);

            // Trả về PartialView với đường dẫn cụ thể trong thư mục Layouts
            return PartialView("Layouts/_MediaPartial", medias);
        }
    }
}