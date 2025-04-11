using Microsoft.AspNetCore.Mvc;
using Web_BTL.BusinessLogicLayer.Services;

namespace Web_BTL.Controllers {
    // Controller xử lý các yêu cầu liên quan đến media
    public class MediaController : Controller {
        private readonly IMediaService _mediaService; // Dịch vụ nghiệp vụ xử lý logic liên quan đến media

        // Constructor: Tiêm IMediaService qua Dependency Injection
        public MediaController(IMediaService mediaService) {
            _mediaService = mediaService;
        }

        // Hiển thị danh sách media phân trang với bộ lọc theo thể loại
        public async Task<IActionResult> Index(int? mid, int page = 1) {
            // Gọi tầng nghiệp vụ để lấy danh sách media phân trang
            var (medias, pageNumbers, currentPage) = await _mediaService.GetPagedMediasAsync(mid, page);

            // Gán dữ liệu vào ViewBag để hiển thị trong View
            ViewBag.PageNumbers = pageNumbers; // Tổng số trang
            ViewBag.CurrentPage = currentPage; // Trang hiện tại

            // Trả về View với danh sách media phân trang
            return View(medias);
        }

        // Lọc media theo thể loại và từ khóa, trả về PartialView phân trang
        public async Task<IActionResult> MoviesFilter(int? mid, string? keyword, int? pageindex) {
            // Gọi tầng nghiệp vụ để lấy danh sách media đã lọc và phân trang
            var (medias, pageNumbers, currentPage) = await _mediaService.GetFilteredPagedMediasAsync(mid, keyword, pageindex);

            // Gán dữ liệu vào ViewBag để sử dụng trong PartialView
            ViewBag.PageNumbers = pageNumbers; // Tổng số trang
            ViewBag.mid = mid; // ID thể loại (nếu có)
            ViewBag.keyword = keyword; // Từ khóa tìm kiếm (nếu có)

            // Trả về PartialView với danh sách media đã lọc
            return PartialView("_MediaPartial", medias);
        }
    }
}