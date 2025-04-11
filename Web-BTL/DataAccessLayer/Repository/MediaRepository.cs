using Microsoft.EntityFrameworkCore;
using Web_BTL.DataAccessLayer.Models;


namespace Web_BTL.DataAccessLayer.Repository {
    // Lớp triển khai truy xuất dữ liệu từ cơ sở dữ liệu cho media
    public class MediaRepository : IMediaRepository {
        private readonly DBXemPhimContext _db; // DbContext để truy cập cơ sở dữ liệu

        // Constructor: Tiêm DBXemPhimContext qua Dependency Injection
        public MediaRepository(DBXemPhimContext db) {
            _db = db;
        }

        // Lấy danh sách media dưới dạng IQueryable để hỗ trợ lọc động
        public IQueryable<MediaModel> GetMediasQueryable() {
            return _db.Medias
                .Include(m => m.Genres); // Bao gồm thông tin thể loại liên quan để lọc
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