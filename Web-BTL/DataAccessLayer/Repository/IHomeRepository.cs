using Web_BTL.DataAccessLayer.Models;

namespace Web_BTL.DataAccessLayer.Repository {
    // Giao diện định nghĩa các phương thức truy xuất dữ liệu cho trang chủ
    public interface IHomeRepository {
        // Lấy tất cả media từ cơ sở dữ liệu
        Task<List<MediaModel>> GetAllMediasAsync();

        // Lấy danh sách media theo loại thể loại (Cartoon, Movie, Series)
        Task<List<MediaModel>> GetMediasByGenreTypeAsync(string genreType);

        // Lấy danh sách media theo ID thể loại
        Task<List<MediaModel>> GetMediasByGenreIdAsync(int genreId);
    }
}