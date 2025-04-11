using Web_BTL.DataAccessLayer.Models;

namespace Web_BTL.BusinessLogicLayer.Services {
    // Giao diện định nghĩa các phương thức nghiệp vụ cho trang chủ
    public interface IHomeService {
        // Lấy dữ liệu cho trang Index (tất cả media và danh sách theo loại)
        Task<(List<MediaModel> AllMedias, List<MediaModel> Cartoons, List<MediaModel> Movies, List<MediaModel> Series)> GetHomeDataAsync();

        // Lấy danh sách media theo ID thể loại cho GenreFilter
        Task<List<MediaModel>> GetMediasByGenreIdAsync(int genreId);

        // Lấy danh sách media hoạt hình cho CartoonMovieFilter
        Task<List<MediaModel>> GetCartoonMediasAsync();
    }
}