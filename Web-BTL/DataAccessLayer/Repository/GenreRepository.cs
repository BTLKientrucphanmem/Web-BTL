using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using Web_BTL.DataAccessLayer.Models;


namespace Web_BTL.DataAccessLayer.Repository {
    // Lớp triển khai truy xuất dữ liệu từ cơ sở dữ liệu cho thể loại và media
    public class GenreRepository : IGenreRepository {
        private readonly DBXemPhimContext _db; // DbContext để truy cập cơ sở dữ liệu

        // Constructor: Tiêm DBXemPhimContext qua Dependency Injection
        public GenreRepository(DBXemPhimContext db) {
            _db = db;
        }

        // Lấy danh sách media dưới dạng IQueryable để hỗ trợ lọc động
        public IQueryable<MediaModel> GetMediasQueryable() {
            return _db.Medias.AsQueryable(); // Trả về IQueryable để tầng nghiệp vụ áp dụng các bộ lọc
        }

        // Lấy danh sách diễn viên dưới dạng SelectListItem cho dropdown
        public async Task<List<SelectListItem>> GetActorSelectListAsync() {
            return await _db.Actors
                .Select(a => new SelectListItem {
                    Text = a.ActorName,   // Tên diễn viên hiển thị trong dropdown
                    Value = a.ActorID.ToString() // Giá trị ID của diễn viên
                })
                .ToListAsync();
        }

        // Lấy danh sách thể loại dưới dạng SelectListItem cho dropdown
        public async Task<List<SelectListItem>> GetGenreSelectListAsync() {
            return await _db.Genres
                .Select(g => new SelectListItem {
                    Text = g.Type,       // Tên thể loại hiển thị trong dropdown
                    Value = g.GenreId.ToString() // Giá trị ID của thể loại
                })
                .ToListAsync();
        }

        // Lấy danh sách chất lượng media duy nhất
        public async Task<List<string>> GetQualityListAsync() {
            return await _db.Medias
                .Select(m => m.MediaQuality) // Lấy tất cả giá trị chất lượng
                .Distinct() // Loại bỏ trùng lặp
                .ToListAsync();
        }

        // Đếm tổng số media dựa trên truy vấn IQueryable
        public async Task<int> CountMediasAsync(IQueryable<MediaModel> medias) {
            return await medias.CountAsync(); // Đếm số lượng media từ truy vấn
        }

        // Lấy danh sách media phân trang từ truy vấn IQueryable
        public async Task<List<MediaModel>> GetPagedMediasAsync(IQueryable<MediaModel> medias, int skip, int take) {
            return await medias
                .Skip(skip) // Bỏ qua số lượng mục được chỉ định
                .Take(take) // Lấy số lượng mục được chỉ định
                .ToListAsync();
        }
    }
}