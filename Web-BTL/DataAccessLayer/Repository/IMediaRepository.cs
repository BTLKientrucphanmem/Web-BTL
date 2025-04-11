using Web_BTL.DataAccessLayer.Models;

namespace Web_BTL.DataAccessLayer.Repository {
    // Giao diện định nghĩa các phương thức truy xuất dữ liệu liên quan đến media
    public interface IMediaRepository {
        // Lấy danh sách media dưới dạng IQueryable để hỗ trợ lọc động
        IQueryable<MediaModel> GetMediasQueryable();

        // Đếm tổng số media dựa trên truy vấn IQueryable
        Task<int> CountMediasAsync(IQueryable<MediaModel> medias);

        // Lấy danh sách media phân trang từ truy vấn IQueryable
        Task<List<MediaModel>> GetPagedMediasAsync(IQueryable<MediaModel> medias, int skip, int take);
    }
}