using Microsoft.AspNetCore.Mvc.Rendering;
using Web_BTL.DataAccessLayer.Models;
using Web_BTL.DataAccessLayer.Repository;
using Microsoft.EntityFrameworkCore;

namespace Web_BTL.DataAccessLayer.Services {
    // Lớp triển khai logic nghiệp vụ liên quan đến thể loại và media
    public class GenreService : IGenreService {
        private readonly IGenreRepository _genreRepository; // Repository truy cập dữ liệu

        // Constructor: Tiêm IGenreRepository qua Dependency Injection
        public GenreService(IGenreRepository genreRepository) {
            _genreRepository = genreRepository;
        }

        // Lấy danh sách media đã lọc và dữ liệu dropdown cho Index
        public async Task<(List<MediaModel> Medias, List<SelectListItem> Actors, List<SelectListItem> Genres, List<string> Qualities)> GetFilteredMediasAsync(int? actorId, int? genreId, string quality, string duration) {
            // Lấy danh sách media dưới dạng IQueryable từ tầng dữ liệu
            var medias = _genreRepository.GetMediasQueryable();

            // Áp dụng bộ lọc theo diễn viên nếu có
            if (actorId.HasValue)
                medias = medias.Where(m => m.Actors.Any(a => a.Actor.ActorID == actorId.Value));

            // Áp dụng bộ lọc theo thể loại nếu có
            if (genreId.HasValue)
                medias = medias.Where(m => m.Genres.Any(g => g.GenreId == genreId.Value));

            // Áp dụng bộ lọc theo chất lượng nếu có
            if (!string.IsNullOrEmpty(quality))
                medias = medias.Where(m => m.MediaQuality == quality);

            // Áp dụng bộ lọc theo thời lượng nếu có
            if (!string.IsNullOrEmpty(duration)) {
                switch (duration) {
                    case "short":
                        medias = medias.Where(m => m.MediaDuration.HasValue && m.MediaDuration.Value <= TimeSpan.FromMinutes(60));
                        break;
                    case "medium":
                        medias = medias.Where(m => m.MediaDuration.HasValue && m.MediaDuration.Value > TimeSpan.FromMinutes(60) && m.MediaDuration.Value <= TimeSpan.FromMinutes(120));
                        break;
                    case "long":
                        medias = medias.Where(m => m.MediaDuration.HasValue && m.MediaDuration.Value > TimeSpan.FromMinutes(120));
                        break;
                }
            }

            // Lấy danh sách media đã lọc
            var mediaList = await medias.ToListAsync();

            // Lấy dữ liệu dropdown từ tầng dữ liệu
            var actors = await _genreRepository.GetActorSelectListAsync();
            var genres = await _genreRepository.GetGenreSelectListAsync();
            var qualities = await _genreRepository.GetQualityListAsync();

            // Trả về tuple chứa danh sách media và dữ liệu dropdown
            return (mediaList, actors, genres, qualities);
        }

        // Lấy danh sách media phân trang với tìm kiếm cho AllMedias
        public async Task<(List<MediaModel> Medias, int PageNumbers, int CurrentPage, string SearchTerm)> GetPagedMediasAsync(int? pageIndex, string searchTerm) {
            // Lấy danh sách media dưới dạng IQueryable từ tầng dữ liệu
            var medias = _genreRepository.GetMediasQueryable();

            // Áp dụng tìm kiếm nếu có từ khóa
            if (!string.IsNullOrEmpty(searchTerm))
                medias = medias.Where(m => m.MediaName.Contains(searchTerm));

            // Thiết lập phân trang
            int pageSize = 8; // Số lượng media mỗi trang
            int totalItems = await _genreRepository.CountMediasAsync(medias); // Tổng số media
            int pageNumbers = (int)Math.Ceiling(totalItems / (double)pageSize); // Tính tổng số trang
            int currentPage = pageIndex ?? 1; // Trang hiện tại, mặc định là 1 nếu không có pageIndex

            // Tính số lượng mục cần bỏ qua
            int skip = (currentPage - 1) * pageSize;

            // Lấy danh sách media phân trang từ tầng dữ liệu
            var pagedMedias = await _genreRepository.GetPagedMediasAsync(medias, skip, pageSize);

            // Trả về tuple chứa danh sách media phân trang và thông tin phân trang
            return (pagedMedias, pageNumbers, currentPage, searchTerm);
        }

        // Lấy danh sách media theo thể loại với số lượng giới hạn cho MoviesFilter
        public async Task<List<MediaModel>> GetMediasByGenreIdAsync(int? genreId, int pageSize = 8) {
            // Lấy danh sách media dưới dạng IQueryable từ tầng dữ liệu
            var medias = _genreRepository.GetMediasQueryable();

            // Áp dụng bộ lọc theo thể loại nếu có
            if (genreId.HasValue)
                medias = medias.Where(m => m.Genres.Any(g => g.GenreId == genreId.Value));

            // Lấy danh sách media với số lượng giới hạn
            return await medias.Take(pageSize).ToListAsync();
        }

        // Lấy danh sách media theo thể loại cho FilteredMedias
        public async Task<List<MediaModel>> GetFilteredMediasByGenreIdAsync(int genreId) {
            // Lấy danh sách media dưới dạng IQueryable từ tầng dữ liệu
            var medias = _genreRepository.GetMediasQueryable();

            // Áp dụng bộ lọc theo thể loại
            medias = medias.Where(m => m.Genres.Any(g => g.GenreId == genreId));

            // Trả về danh sách media đã lọc
            return await medias.ToListAsync();
        }
    }
}