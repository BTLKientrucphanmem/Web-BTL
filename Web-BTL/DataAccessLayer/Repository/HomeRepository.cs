using Microsoft.EntityFrameworkCore;
using Web_BTL.DataAccessLayer.Models;


namespace Web_BTL.DataAccessLayer.Repository {
    // Lớp triển khai truy xuất dữ liệu từ cơ sở dữ liệu cho trang chủ
    public class HomeRepository : IHomeRepository {
        private readonly DBXemPhimContext _db; // DbContext để truy cập cơ sở dữ liệu

        // Constructor: Tiêm DBXemPhimContext qua Dependency Injection
        public HomeRepository(DBXemPhimContext db) {
            _db = db;
        }

        // Lấy tất cả media từ cơ sở dữ liệu
        public async Task<List<MediaModel>> GetAllMediasAsync() {
            return await _db.Medias.ToListAsync(); // Trả về danh sách tất cả media
        }

        // Lấy danh sách media theo loại thể loại (Cartoon, Movie, Series)
        public async Task<List<MediaModel>> GetMediasByGenreTypeAsync(string genreType) {
            return await _db.Medias
                .Include(m => m.Genres) // Bao gồm thông tin thể loại liên quan
                .Where(m => m.Genres.Any(g => g.Type == genreType)) // Lọc media theo loại thể loại
                .ToListAsync();
        }

        // Lấy danh sách media theo ID thể loại
        public async Task<List<MediaModel>> GetMediasByGenreIdAsync(int genreId) {
            return await _db.Medias
                .Include(m => m.Genres) // Bao gồm thông tin thể loại liên quan
                .Where(m => m.Genres.Any(g => g.GenreId == genreId)) // Lọc media theo ID thể loại
                .ToListAsync();
        }
    }
}