using Web_BTL.DataAccessLayer.Models;

namespace Web_BTL.DataAccessLayer.Repository {
    // Giao diện định nghĩa các phương thức truy xuất và cập nhật dữ liệu liên quan đến phim
    public interface IMovieRepository {
        // Lấy thông tin media theo ID
        Task<MediaModel?> GetMediaByIdAsync(int mediaId);

        // Lấy thông tin khách hàng theo email
        Task<CustomerModel?> GetCustomerByEmailAsync(string email);

        // Kiểm tra xem media đã có trong danh sách của khách hàng chưa
        Task<bool> IsMediaInWatchListAsync(int watchListId, int mediaId);

        // Thêm media mới vào danh sách của khách hàng
        Task AddMediaToWatchListAsync(ListMediaModel listMedia);

        // Lấy thông tin ListMedia theo WatchListId và MediaId
        Task<ListMediaModel?> GetListMediaAsync(int watchListId, int mediaId);

        // Cập nhật thông tin ListMedia (ví dụ: IsWatched, Favorite)
        Task UpdateListMediaAsync(ListMediaModel listMedia);
    }
}