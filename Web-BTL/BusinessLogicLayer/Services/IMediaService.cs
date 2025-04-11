using Web_BTL.DataAccessLayer.Models;

namespace Web_BTL.BusinessLogicLayer.Services {
    // Giao diện định nghĩa các phương thức nghiệp vụ liên quan đến media
    public interface IMediaService {
        // Lấy danh sách media phân trang với bộ lọc theo thể loại cho Index
        Task<(List<MediaModel> Medias, int PageNumbers, int CurrentPage)> GetPagedMediasAsync(int? genreId, int page = 1);

        // Lấy danh sách media phân trang với bộ lọc theo thể loại và từ khóa cho MoviesFilter
        Task<(List<MediaModel> Medias, int PageNumbers, int CurrentPage)> GetFilteredPagedMediasAsync(int? genreId, string? keyword, int? pageIndex);
    }
}