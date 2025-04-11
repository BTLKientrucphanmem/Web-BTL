using Microsoft.AspNetCore.Mvc;
using Web_BTL.DataAccessLayer.Models;
using Web_BTL.BusinessLogicLayer.Services;

namespace Web_BTL.BusinessLogicLayer.Controllers {
    public class AdminController : Controller {
        private readonly IAdminService _adminService; // Dịch vụ xử lý logic nghiệp vụ cho Admin

        // Constructor: Tiêm IAdminService qua Dependency Injection
        public AdminController(IAdminService adminService) {
            _adminService = adminService;
        }

        // Hiển thị danh sách tất cả các media
        public async Task<IActionResult> Index() {
            // Kiểm tra xem đã đăng nhập chưa thông qua Session
            if (HttpContext.Session.GetString("LogIn Session") == null)
                return NotFound("Không tìm thấy trang"); // Trả về lỗi nếu chưa đăng nhập

            // Lấy danh sách media từ tầng nghiệp vụ
            var medias = await _adminService.GetAllMediasAsync();
            return View(medias); // Trả về View với danh sách media
        }

        // Hiển thị form thêm media mới (GET)
        [HttpGet]
        public IActionResult AddMedia() {
            // Kiểm tra xem đã đăng nhập chưa
            if (HttpContext.Session.GetString("LogIn Session") == null)
                return NotFound("Không tìm thấy trang");

            // Tạo ViewBag chứa danh sách thể loại để hiển thị trong form
            ViewBag.AllGenres = _adminService.GetGenreSelectList();
            return View(); // Trả về View để thêm media
        }

        // Xử lý thêm media mới (POST)
        [HttpPost]
        public async Task<IActionResult> AddMedia(MediaModel media, IFormFile image, IFormFile banner, IFormFile video, List<int> SelectedGenreId) {
            // Kiểm tra tính hợp lệ của dữ liệu đầu vào
            if (!ModelState.IsValid) {
                ViewBag.AllGenres = _adminService.GetGenreSelectList(); // Tải lại danh sách thể loại nếu lỗi
                return View(media); // Trả về View với dữ liệu đã nhập để sửa lỗi
            }

            // Gọi tầng nghiệp vụ để thêm media
            var (success, errorMessage, updatedMedia) = await _adminService.AddMediaAsync(media, image, banner, video, SelectedGenreId);
            if (success)
                return RedirectToAction("Index"); // Chuyển hướng về trang danh sách nếu thành công

            // Nếu thất bại, hiển thị thông báo lỗi
            ModelState.AddModelError(string.Empty, errorMessage);
            ViewBag.AllGenres = _adminService.GetGenreSelectList();
            return View(media); // Trả về View với thông báo lỗi
        }

        // Hiển thị form chỉnh sửa media (GET)
        [HttpGet]
        public async Task<IActionResult> EditMedia(int? mid) {
            // Kiểm tra xem ID media có hợp lệ và đã đăng nhập chưa
            if (mid == null || HttpContext.Session.GetString("LogIn Session") == null)
                return RedirectToAction(nameof(Index)); // Quay lại trang danh sách nếu không hợp lệ

            // Lấy thông tin media từ tầng nghiệp vụ
            var (success, media) = await _adminService.GetMediaByIdAsync(mid.Value);
            if (!success || media == null)
                return RedirectToAction(nameof(Index)); // Quay lại nếu không tìm thấy media

            // Tạo ViewBag chứa danh sách thể loại và diễn viên để hiển thị trong form
            ViewBag.AllGenres = _adminService.GetGenreSelectList();
            ViewBag.AllActors = _adminService.GetActorSelectList();
            return View(media); // Trả về View để chỉnh sửa media
        }

        // Xử lý chỉnh sửa media (POST)
        [HttpPost]
        public async Task<IActionResult> EditMedia(int mid, MediaModel model, IFormFile? image, IFormFile? banner, IFormFile? video, List<int> SelectedGenreId, List<int> SelectedActorId, List<int> SelectedActorMain) {
            // Kiểm tra tính hợp lệ của dữ liệu đầu vào
            if (!ModelState.IsValid) {
                ViewBag.AllGenres = _adminService.GetGenreSelectList(); // Tải lại danh sách thể loại
                ViewBag.AllActors = _adminService.GetActorSelectList(); // Tải lại danh sách diễn viên
                return View(model); // Trả về View với dữ liệu đã nhập để sửa lỗi
            }

            // Gọi tầng nghiệp vụ để chỉnh sửa media
            var (success, errorMessage, updatedMedia) = await _adminService.EditMediaAsync(mid, model, image, banner, video, SelectedGenreId, SelectedActorId, SelectedActorMain);
            if (success)
                return RedirectToAction("Index"); // Chuyển hướng về trang danh sách nếu thành công

            // Nếu thất bại, hiển thị thông báo lỗi
            ModelState.AddModelError(string.Empty, errorMessage);
            ViewBag.AllGenres = _adminService.GetGenreSelectList();
            ViewBag.AllActors = _adminService.GetActorSelectList();
            return View(model); // Trả về View với thông báo lỗi
        }

        // Xử lý xóa media (POST)
        [HttpPost]
        public async Task<IActionResult> DeleteMedia(int? mid, bool YesNo = false) {
            // Kiểm tra xem ID media có hợp lệ và người dùng có đồng ý xóa không
            if (mid != null && YesNo) {
                var success = await _adminService.DeleteMediaAsync(mid.Value); // Gọi tầng nghiệp vụ để xóa media
                if (!success)
                    return NotFound(); // Trả về lỗi nếu xóa thất bại
            }
            return RedirectToAction(nameof(Index)); // Chuyển hướng về trang danh sách
        }

        // Hiển thị danh sách thể loại (Genre)
        public async Task<IActionResult> ListGenre() {
            // Lấy danh sách thể loại từ tầng nghiệp vụ
            var genres = await _adminService.GetAllGenresAsync();
            return View(genres); // Trả về View với danh sách thể loại
        }

        // Hiển thị form thêm thể loại mới (GET)
        [HttpGet]
        public IActionResult AddGenre() {
            return View(); // Trả về View để thêm thể loại
        }

        // Xử lý thêm thể loại mới (POST)
        [HttpPost]
        public async Task<IActionResult> AddGenre(GenreModel genre) {
            // Kiểm tra tính hợp lệ của dữ liệu đầu vào
            if (ModelState.IsValid) {
                var success = await _adminService.AddGenreAsync(genre); // Gọi tầng nghiệp vụ để thêm thể loại
                if (success)
                    return RedirectToAction(nameof(ListGenre)); // Chuyển hướng về danh sách nếu thành công
            }
            return View(genre); // Trả về View với dữ liệu đã nhập nếu thất bại
        }

        // Xử lý chỉnh sửa thể loại (POST)
        [HttpPost]
        public async Task<IActionResult> EditGenre([FromBody] GenreModel genre) {
            // Gọi tầng nghiệp vụ để chỉnh sửa thể loại
            var success = await _adminService.EditGenreAsync(genre);
            return Json(new { success }); // Trả về JSON với trạng thái thành công/thất bại
        }

        // Xử lý xóa thể loại (POST)
        [HttpPost]
        public async Task<IActionResult> DeleteGenre(int? gid, bool YesNo = false) {
            // Kiểm tra xem ID thể loại có hợp lệ và người dùng có đồng ý xóa không
            if (gid != null && YesNo) {
                var success = await _adminService.DeleteGenreAsync(gid.Value); // Gọi tầng nghiệp vụ để xóa thể loại
                if (!success)
                    return NotFound(); // Trả về lỗi nếu xóa thất bại
            }
            return RedirectToAction(nameof(ListGenre)); // Chuyển hướng về danh sách
        }

        // Hiển thị danh sách diễn viên (Actor)
        public async Task<IActionResult> ListActor() {
            // Lấy danh sách diễn viên từ tầng nghiệp vụ
            var actors = await _adminService.GetAllActorsAsync();
            return View(actors); // Trả về View với danh sách diễn viên
        }

        // Hiển thị form thêm diễn viên mới (GET)
        [HttpGet]
        public IActionResult AddActor() {
            return View(); // Trả về View để thêm diễn viên
        }

        // Xử lý thêm diễn viên mới (POST)
        [HttpPost]
        public async Task<IActionResult> AddActor(ActorModel actor) {
            // Kiểm tra tính hợp lệ của dữ liệu đầu vào
            if (ModelState.IsValid) {
                var success = await _adminService.AddActorAsync(actor); // Gọi tầng nghiệp vụ để thêm diễn viên
                if (success)
                    return RedirectToAction(nameof(ListActor)); // Chuyển hướng về danh sách nếu thành công
            }
            return View(actor); // Trả về View với dữ liệu đã nhập nếu thất bại
        }

        // Xử lý chỉnh sửa tên diễn viên (POST)
        [HttpPost]
        public async Task<IActionResult> EditActorName([FromBody] ActorModel actor) {
            // Gọi tầng nghiệp vụ để chỉnh sửa tên diễn viên
            var success = await _adminService.EditActorAsync(actor);
            return Json(new { success }); // Trả về JSON với trạng thái thành công/thất bại
        }

        // Xử lý chỉnh sửa ngày sinh diễn viên (POST)
        [HttpPost]
        public async Task<IActionResult> EditActorDate([FromBody] ActorModel actor) {
            // Gọi tầng nghiệp vụ để chỉnh sửa ngày sinh diễn viên
            var success = await _adminService.EditActorAsync(actor);
            return Json(new { success }); // Trả về JSON với trạng thái thành công/thất bại
        }

        // Xử lý xóa diễn viên (POST)
        [HttpPost]
        public async Task<IActionResult> DeleteActor(int? aid, bool YesNo = false) {
            // Kiểm tra xem ID diễn viên có hợp lệ và người dùng có đồng ý xóa không
            if (aid != null && YesNo) {
                var success = await _adminService.DeleteActorAsync(aid.Value); // Gọi tầng nghiệp vụ để xóa diễn viên
                if (!success)
                    return NotFound(); // Trả về lỗi nếu xóa thất bại
            }
            return RedirectToAction(nameof(ListActor)); // Chuyển hướng về danh sách
        }

        // Hiển thị danh sách khách hàng (Customer)
        public async Task<IActionResult> ListCustomer() {
            // Lấy danh sách khách hàng từ tầng nghiệp vụ
            var customers = await _adminService.GetAllCustomersAsync();
            return View(customers); // Trả về View với danh sách khách hàng
        }

        // Xử lý bật/tắt trạng thái khách hàng (POST)
        [HttpPost]
        public async Task<IActionResult> ToggleUserState(int customerId) {
            // Gọi tầng nghiệp vụ để bật/tắt trạng thái khách hàng
            var (success, newState) = await _adminService.ToggleCustomerStateAsync(customerId);
            return Json(new { success, newState }); // Trả về JSON với trạng thái thành công và giá trị mới
        }

        // Tải danh sách media theo thể loại hoặc tất cả (GET)
        [HttpGet]
        public async Task<IActionResult> LoadMediaList(string type, string id) {
            // Nếu yêu cầu là tải media theo thể loại
            if (type == "genre" && int.TryParse(id, out int genreId)) {
                var mediaList = await _adminService.GetMediasByGenreAsync(genreId); // Lấy media theo thể loại
                return PartialView("MediaTable", mediaList); // Trả về PartialView với danh sách media
            }
            // Nếu yêu cầu là tải tất cả media
            if (type == "all") {
                var mediaList = await _adminService.GetAllMediasAsync(); // Lấy tất cả media
                return PartialView("MediaTable", mediaList); // Trả về PartialView với danh sách media
            }
            return NotFound(); // Trả về lỗi nếu type không hợp lệ
        }
    }
}