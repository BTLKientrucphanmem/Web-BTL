using Web_BTL.DataAccessLayer.Models;

namespace Web_BTL.BusinessLogicLayer.Services{
    // Giao diện định nghĩa các phương thức nghiệp vụ liên quan đến phim
    public interface IMovieService {
        // Lấy thông tin media và thêm vào danh sách xem nếu chưa có (cho Index)
        Task<MediaModel?> GetMediaAndUpdateWatchListAsync(int movieId, string? userEmail);

        // Lấy thông tin media và cập nhật trạng thái đã xem (cho Watching)
        Task<MediaModel?> GetMediaAndMarkAsWatchedAsync(int movieId, string? userEmail);

        // Thêm media vào danh sách yêu thích (cho AddFavourite)
        Task<(bool Success, bool IsRedirectToLogin)> AddFavouriteAsync(int mediaId, string? userEmail);
    }
}