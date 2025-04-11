using Microsoft.EntityFrameworkCore;
using Web_BTL.DataAccessLayer.Models;


namespace Web_BTL.DataAccessLayer.Repository {
    // Lớp triển khai truy xuất và cập nhật dữ liệu từ cơ sở dữ liệu cho phim
    public class MovieRepository : IMovieRepository {
        private readonly DBXemPhimContext _db; // DbContext để truy cập cơ sở dữ liệu

        // Constructor: Tiêm DBXemPhimContext qua Dependency Injection
        public MovieRepository(DBXemPhimContext db) {
            _db = db;
        }

        // Lấy thông tin media theo ID
        public async Task<MediaModel?> GetMediaByIdAsync(int mediaId) {
            return await _db.Medias
                .FirstOrDefaultAsync(m => m.MediaId == mediaId); // Trả về media khớp với mediaId hoặc null
        }

        // Lấy thông tin khách hàng theo email
        public async Task<CustomerModel?> GetCustomerByEmailAsync(string email) {
            return await _db.Customers
                .FirstOrDefaultAsync(c => c.UserEmail == email); // Trả về khách hàng khớp với email hoặc null
        }

        // Kiểm tra xem media đã có trong danh sách của khách hàng chưa
        public async Task<bool> IsMediaInWatchListAsync(int watchListId, int mediaId) {
            return await _db.ListMedia
                .AnyAsync(lm => lm.WatchListId == watchListId && lm.MediaId == mediaId); // Trả về true nếu media đã tồn tại
        }

        // Thêm media mới vào danh sách của khách hàng
        public async Task AddMediaToWatchListAsync(ListMediaModel listMedia) {
            _db.ListMedia.Add(listMedia); // Thêm bản ghi mới vào bảng ListMedia
            await _db.SaveChangesAsync(); // Lưu thay đổi vào cơ sở dữ liệu
        }

        // Lấy thông tin ListMedia theo WatchListId và MediaId
        public async Task<ListMediaModel?> GetListMediaAsync(int watchListId, int mediaId) {
            return await _db.ListMedia
                .FirstOrDefaultAsync(lm => lm.WatchListId == watchListId && lm.MediaId == mediaId); // Trả về bản ghi khớp hoặc null
        }

        // Cập nhật thông tin ListMedia (ví dụ: IsWatched, Favorite)
        public async Task UpdateListMediaAsync(ListMediaModel listMedia) {
            _db.ListMedia.Update(listMedia); // Cập nhật bản ghi trong bảng ListMedia
            await _db.SaveChangesAsync(); // Lưu thay đổi vào cơ sở dữ liệu
        }
    }
}