using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Web_BTL.Models;
using Web_BTL.Repository;
using Web_BTL.UploadFile;

namespace Web_BTL.Controllers
{
    public class AdminController : Controller
    {
        private readonly DBXemPhimContext _datacontext; // luồng đọc dữ liệu từ database
        private readonly IWebHostEnvironment _environment; // môi trường web
        private readonly SaveImageVideo _save; // service dùng để lưu ảnh và video
        public AdminController(DBXemPhimContext datacontext, IWebHostEnvironment environment, SaveImageVideo save)
        {
            // Gán các giá trị service ban đầu
            _datacontext = datacontext;
            _environment = environment;
            _save = save;
        }
        public IActionResult Index()
        {
            // kiểm tra xem đã có dữ liệu đăng nhập chưa
            if (HttpContext.Session.GetString("LogIn Session") == null)
                return NotFound("Không tìm thấy trang");
            // lấy dữ liệu từ CSDL để hiển thị lên view
            var medias = _datacontext.Medias.ToList();
            return View(medias);
        }
        [HttpGet]
        public IActionResult AddMedia()
        {
            // kiểm tra xem tài khoản hiện thời có phải tài khoản admin không và quyền của admin có đủ để thực hiện thao tác không
            string role = HttpContext.Session.GetString("Admin");
            if (role == null || role != Role.SuperAdmin.ToString() 
                && role != Role.Movie_Management.ToString())
                return NotFound("Quyền hạn của bạn không đủ");
            CreateViewBag();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddMedia(MediaModel media, IFormFile image, IFormFile banner, IFormFile video, List<int> SelectedGenreId)
        {
            // lấy ra email và quyền của tài khoản admin
            var email = HttpContext.Session.GetString("LogIn Session");
            if (email == null) return NotFound("Không tìm thấy trang");
            var role = HttpContext.Session.GetString("Admin");
            if (role != Role.SuperAdmin.ToString()
                && role != Role.Movie_Management.ToString())
                return NotFound("Quyền hạn của bạn không đủ");
            if (ModelState.IsValid)
            {
                if (email == null) return NotFound("Hiện trang này không khả dụng"); // nếu không có email tức là chưa đăng nhập
                // lưu image của phim
                if (image != null && image.Length > 0)
                    media.MediaImagePath = await _save.SaveImageAsync(_environment, "images/medias", "", media.MediaName, image);
                // lưu banner của phim
                if (banner != null && banner.Length > 0)
                    media.MediaBannerPath = await _save.SaveImageAsync(_environment, "images/banners", "", media.MediaName + "banner", banner);
                // lưu video phim
                if (video != null && video.Length > 0)
                {
                    var resule = await _save.SaveVideoAsync(_environment, "videos", "", media.MediaName + media.MediaQuality, video, true);
                    media.MediaUrl = resule.videoName;
                    media.MediaDuration = resule.duration;
                }
                // kiểm tra xem đã chọn mục thể loại phim chưa đây là mục bắt buộc phải chọn
                if (SelectedGenreId.Count == 0)
                {
                    Console.WriteLine("Lỗi ở SelectedGenreId.Count == 0");
                    CreateViewBag();
                    return View(media);
                }
                else
                    // lưu những thể loại đã được chọn lại
                    foreach (var item in SelectedGenreId)
                    {
                        var g = await _datacontext.Genres.FirstOrDefaultAsync(g => g.GenreId == item);
                        if (g != null) media.Genres.Add(g);
                    }
                // thêm media vào cơ sở dữ liệu
                await _datacontext.Medias.AddAsync(media);
                await _datacontext.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            else
            {
                // Lấy lỗi từ model state
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage).ToList();
                foreach (var error in errors)
                    Console.WriteLine(error);// Kiểm tra lỗi trên console (có thể kiểm tra lỗi bằng phương thức khác)
            }
            CreateViewBag();
            return View(media);
        }
        // tạo các ViewBag cho các action sử dụng (hầu như đều dùng) trong controller Admin
        private void CreateViewBag()
        {
 
            // lấy ra 1 danh sách các genre(thể loại trong CSDL)
            ViewBag.AllGenres = new List<SelectListItem>();
            var genres = _datacontext.Genres.ToList();
            ViewBag.AllGenres.AddRange(genres.Select(g => new SelectListItem // tạo list item để lựa chon genre cho media
            {
                Text = g.Type,
                Value = g.GenreId.ToString()
            }));
            
        }
        // gọi đến Edit media
        [HttpGet]
        public IActionResult EditMedia(int? mid)
        {
            // kiểm tra đây có phải tài khoản admin không và có đủ quyền chỉnh sửa không
            var email = HttpContext.Session.GetString("LogIn Session");
            if (email == null) return NotFound("Không tìm thấy trang");
            var role = HttpContext.Session.GetString("Admin");
            if (role != Role.SuperAdmin.ToString()
                && role != Role.Movie_Management.ToString())
                return NotFound("Quyền hạn của bạn không đủ");
            var actors = _datacontext.Actors.ToList();
            ViewBag.AllActors = new List<SelectListItem>();
            ViewBag.AllActors.AddRange(actors.Select(a => new SelectListItem
            {
                Text = a.ActorName,
                Value = a.ActorID.ToString()
            }));
            if (mid == null)
                return RedirectToAction(nameof(Index));
            var media = _datacontext.Medias.FirstOrDefault(m => m.MediaId == mid);
            CreateViewBag();
            return View(media);
        }
        // lấy dữ liệu media đã sửa về
        [HttpPost]
        public async Task<IActionResult> EditMedia(int mid, MediaModel model, 
            IFormFile? image, IFormFile? banner, IFormFile? video, List<int> SelectedGenreId, List<int> SelectedActorId, List<int> SelectedActorMain)
        {
            if (mid != model.MediaId) return NotFound();
            var media = await _datacontext.Medias.FirstOrDefaultAsync(m => m.MediaId == mid);
            if (media == null || model == null) return RedirectToAction(nameof(Index));
            if (ModelState.IsValid)
            {
                // cập nhật lại các dữ liệu đã sửa
                media.MediaName = model.MediaName;
                media.MediaDescription = model.MediaDescription;
                media.MediaQuality = model.MediaQuality;
                media.ReleaseDate = model.ReleaseDate;
                media.MediaAgeRating = model.MediaAgeRating;
                await _datacontext.SaveChangesAsync();
                
                if (image != null && image.Length > 0)
                    media.MediaImagePath = await _save.SaveImageAsync(_environment, "images/medias", "", media.MediaName, image);
                if (banner != null && banner.Length > 0)
                    media.MediaBannerPath = await _save.SaveImageAsync(_environment, "images/banners", "", media.MediaName + "banner", banner);
                if (video != null && video.Length > 0)
                {
                    var resule = await _save.SaveVideoAsync(_environment, "videos", "", media.MediaName + media.MediaQuality, video, true);
                    media.MediaUrl = resule.videoName;
                    media.MediaDuration = resule.duration;
                }
                if (SelectedActorId.Count > 0) // nếu các actor được chọn mới có sự thay đổi còn lại có thể ko
                {
                    var ra = await _datacontext.Actor_Medias.Where(a => a.MediaId == mid).ToListAsync();
                    _datacontext.Actor_Medias.RemoveRange(ra);
                    foreach (var id in SelectedActorId)
                    {
                        
                        var a = await _datacontext.Actor_Medias.FirstOrDefaultAsync(am => am.MediaId == model.MediaId && am.ActorId == id);
                        if (a == null)
                        {
                            await _datacontext.Actor_Medias.AddAsync(new Actor_MediaModel
                            {
                                MediaId = mid,
                                Media = media,
                                ActorId = id,
                                Actor = await _datacontext.Actors.FindAsync(id)
                            });
                        }
                    }
                    await _datacontext.SaveChangesAsync();
                    foreach (var main in SelectedActorMain) // xem xem đã có actor nào được chọn là diễn viên chính của phim này chưa
                    {
                        var a = await _datacontext.Actor_Medias.FirstOrDefaultAsync(am => am.MediaId == model.MediaId && am.ActorId == main);
                        if (a == null)
                            await _datacontext.Actor_Medias.AddAsync(new Actor_MediaModel
                            {
                                MediaId = mid,
                                Media = media,
                                ActorId = main,
                                Actor = await _datacontext.Actors.FindAsync(main)
                            });
                        a.IsMain = true;
                    }
                    await _datacontext.SaveChangesAsync();
                }
                if (SelectedGenreId.Count > 0) // sửa lại genre(thể loại) nếu muốn
                {
                    await _datacontext.Database.ExecuteSqlRawAsync($"DELETE FROM Media_Genre WHERE MediaId = {media.MediaId}");
                    await _datacontext.SaveChangesAsync();
                    foreach (var item in SelectedGenreId)
                    {
                        var g = await _datacontext.Genres.FirstOrDefaultAsync(g => g.GenreId == item);
                        if (g != null) media.Genres.Add(g);
                    }
                }
                await _datacontext.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            // khi sửa lại media gặp 1 lỗi bất kì
            CreateViewBag();
            ViewBag.AllActors = new List<SelectListItem>();
            var actors = _datacontext.Actors.ToList();
            ViewBag.AllActors.AddRange(actors.Select(a => new SelectListItem
            {
                Text = a.ActorName,
                Value = a.ActorID.ToString()
            }));
            return View(model);
        }
        private bool mediaExists(int id)
        {
            return (_datacontext.Medias?.Any(e => e.MediaId == id)).GetValueOrDefault();
        }
        // xoá media
        [HttpPost]
        public async Task<IActionResult> DeleteMedia(int? mid, bool YesNo = false)
        {
            if (mid != null && YesNo) // kiểm tra xem đã có mid chưa và có được đồng ý hay không
            {
                var media = await _datacontext.Medias.FirstAsync(m => m.MediaId == mid);
                _save.DeleteFile(_environment, "images/medias", media.MediaImagePath); // xoá ảnh của media trong wwwroot
                _save.DeleteFile(_environment, "videos", media.MediaUrl); // xoá media trong wwwroot
                _datacontext.Medias.Remove(media);
                await _datacontext.SaveChangesAsync();
                Console.WriteLine("da xoa thanh cong");
            }
            return RedirectToAction(nameof(Index));
        }
        public IActionResult ListGenre()
        {
            var genres = _datacontext.Genres.ToList(); // danh sách các genre(thể loại) trong CSDL
            return View(genres);
        }
        [HttpGet]
        public IActionResult AddGenre()
        {
            return View();
        }
        // thêm 1 genre(thể loại mới)
        [HttpPost]
        public async Task<IActionResult> AddGenre(GenreModel genre)
        {
            if (genre == null) return View(genre);
            await _datacontext.Genres.AddAsync(genre);
            await _datacontext.SaveChangesAsync();
            return RedirectToAction(nameof(ListGenre));
        }
        // sửa thể loại
        [HttpPost]
        public IActionResult EditGenre([FromBody]GenreModel genre)
        {
            var model = _datacontext.Genres.FirstOrDefault(g => g.GenreId == genre.GenreId);
            if (model != null) // kiểm tra genre(thể loại đã được post về chưa)
            {
                model.Type = genre.Type;
                _datacontext.SaveChanges();
                return Json(new { success = true }); // đã sửa thành công
            }
            return Json(new { success = false}); // chưa thể sửa genre(thể loại)
        }
        // xoá đi 1 thể loại
        [HttpPost]
        public async Task<IActionResult> DeleteGenre(int? gid, bool YesNo = false)
        {
            if (gid != null && YesNo) // nếu đông ý xoá
            {
                var genre = _datacontext.Genres.FirstOrDefault(g => g.GenreId == gid);
                _datacontext.Genres.Remove(genre);
                await _datacontext.SaveChangesAsync();
            }
            return RedirectToAction(nameof(ListGenre));
        }
        public IActionResult ListActor()
        {
            var actors = _datacontext.Actors.ToList(); // lấy toàn bộ actor có trong CSDL
            return View(actors);
        }
        [HttpGet]
        public IActionResult AddActor()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddActor(ActorModel actor)
        {
            if (actor == null) return View(actor);
            await _datacontext.Actors.AddAsync(actor);
            await _datacontext.SaveChangesAsync();
            return RedirectToAction(nameof(ListActor));
        }
        [HttpPost]
        public IActionResult EditActorName([FromBody]ActorModel actor)
        {
            var model = _datacontext.Actors.Find(actor.ActorID);
            if (model != null)
            {
                model.ActorName = actor.ActorName;
                _datacontext.SaveChanges();
                return Json(new {success = true}); // đã sửa thành công tên actor(diễn viên)
            }
            return Json(new {success = false}); // sửa thất bại
        }
        [HttpPost]
        public IActionResult EditActorDate([FromBody] ActorModel actor)
        {
            if (actor.ActorID == null) return Json(new { success = false });
            var model = _datacontext.Actors.FirstOrDefault(a => a.ActorID == actor.ActorID);
            if (model != null)
            {
                model.AcctorDate = actor.AcctorDate;
                _datacontext.SaveChanges();
                return Json(new { success = true }); // đã sửa thành công ngày sinh của actor
            }
            return Json(new { success = false }); // sửa thất bại
        }
        // xoá 1 actor
        [HttpPost]
        public async Task<IActionResult> DeleteActor(int? aid, bool YesNo = false)
        {
            if (aid != null && YesNo)
            {
                var actor = await _datacontext.Actors.FindAsync(aid);
                _datacontext.Actors.Remove(actor);
                await _datacontext.SaveChangesAsync();
            }
            return RedirectToAction(nameof(ListActor));
        }
        public IActionResult ListCustomer()
        {
            var customers = _datacontext.Customers.ToList(); // lấy toàn bộ thông tin người dùng(customer) trong CSDL
            return View(customers);
        }

        [HttpPost]
        public IActionResult ToggleUserState(int customerId)
        {
            // kiểm tra đây có phải tài khoản admin không và có đủ quyền chỉnh sửa không
            var email = HttpContext.Session.GetString("LogIn Session");
            if (email == null) return NotFound("Không tìm thấy trang");
            var role = HttpContext.Session.GetString("Admin");
            if (role != Role.SuperAdmin.ToString()
                && role != Role.ServicePackage_Management.ToString())
                return NotFound("Quyền hạn của bạn không đủ");
            var customer = _datacontext.Customers.Find(customerId);
            Console.WriteLine("day la ToggleUserState");
            if (customer != null)
            {
                customer.UserState = !customer.UserState;
                _datacontext.SaveChanges();
                return Json(new { success = true, newState = customer.UserState });
            }
            return Json(new { success = false });
        }
        [HttpGet]
        public IActionResult LoadMediaList(string type, string id)
        {
           
            if (type == "genre")
            {
                int gid = int.Parse(id);
                var mediaList = _datacontext.Medias.Where(m => m.Genres.Any(g => g.GenreId == gid)).ToList();
                return PartialView("MediaTable", mediaList);
            }
            if (type == "all")
            {
                var mediaList = _datacontext.Medias.ToList();
                return PartialView("MediaTable", mediaList);
            }
            return NotFound();
        }
       
        
    }
}
