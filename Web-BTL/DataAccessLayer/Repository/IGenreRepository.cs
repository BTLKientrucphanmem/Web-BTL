using Microsoft.AspNetCore.Mvc.Rendering;
using Web_BTL.DataAccessLayer.Models;

namespace Web_BTL.DataAccessLayer.Repository {
    // Giao diện định nghĩa các phương thức truy xuất dữ liệu liên quan đến thể loại và media
    public interface IGenreRepository {
        // Lấy danh sách media dưới dạng IQueryable để hỗ trợ lọc động
        IQueryable<MediaModel> GetMediasQueryable();

        // Lấy danh sách diễn viên dưới dạng SelectListItem cho dropdown
        Task<List<SelectListItem>> GetActorSelectListAsync();

        // Lấy danh sách thể loại dưới dạng SelectListItem cho dropdown
        Task<List<SelectListItem>> GetGenreSelectListAsync();

        // Lấy danh sách chất lượng media duy nhất
        Task<List<string>> GetQualityListAsync();

        // Đếm tổng số media dựa trên truy vấn IQueryable
        Task<int> CountMediasAsync(IQueryable<MediaModel> medias);

        // Lấy danh sách media phân trang từ truy vấn IQueryable
        Task<List<MediaModel>> GetPagedMediasAsync(IQueryable<MediaModel> medias, int skip, int take);
    }
}