using Microsoft.AspNetCore.Mvc.Rendering;
using Web_BTL.DataAccessLayer.Models;

namespace Web_BTL.DataAccessLayer.Services {
    // Giao diện định nghĩa các phương thức nghiệp vụ liên quan đến thể loại và media
    public interface IGenreService {
        // Lấy danh sách media đã lọc và dữ liệu dropdown cho Index
        Task<(List<MediaModel> Medias, List<SelectListItem> Actors, List<SelectListItem> Genres, List<string> Qualities)> GetFilteredMediasAsync(int? actorId, int? genreId, string quality, string duration);

        // Lấy danh sách media phân trang với tìm kiếm cho AllMedias
        Task<(List<MediaModel> Medias, int PageNumbers, int CurrentPage, string SearchTerm)> GetPagedMediasAsync(int? pageIndex, string searchTerm);

        // Lấy danh sách media theo thể loại với số lượng giới hạn cho MoviesFilter
        Task<List<MediaModel>> GetMediasByGenreIdAsync(int? genreId, int pageSize = 8);

        // Lấy danh sách media theo thể loại cho FilteredMedias
        Task<List<MediaModel>> GetFilteredMediasByGenreIdAsync(int genreId);
    }
}