
using Web_BTL.DataAccessLayer.Models;
using Web_BTL.DataAccessLayer.Repository;


namespace Web_BTL.BusinessLogicLayer.Services {
    // Lớp triển khai logic nghiệp vụ liên quan đến media
    public class MediaService : IMediaService {
        private readonly IMediaRepository _mediaRepository; // Repository truy cập dữ liệu
        private const int PageSize = 16; // Số lượng media mỗi trang (hằng số)

        // Constructor: Tiêm IMediaRepository qua Dependency Injection
        public MediaService(IMediaRepository mediaRepository) {
            _mediaRepository = mediaRepository;
        }

        // Lấy danh sách media phân trang với bộ lọc theo thể loại cho Index
        public async Task<(List<MediaModel> Medias, int PageNumbers, int CurrentPage)> GetPagedMediasAsync(int? genreId, int page = 1) {
            // Lấy danh sách media dưới dạng IQueryable từ tầng dữ liệu
            var medias = _mediaRepository.GetMediasQueryable();

            // Áp dụng bộ lọc theo thể loại nếu có
            if (genreId.HasValue)
                medias = medias.Where(m => m.Genres.Any(g => g.GenreId == genreId.Value));

            // Tính tổng số media và số trang
            int totalMedia = await _mediaRepository.CountMediasAsync(medias);
            int pageNumbers = (int)Math.Ceiling(totalMedia / (float)PageSize);

            // Đảm bảo page không nhỏ hơn 1
            int currentPage = page <= 0 ? 1 : page;

            // Tính số lượng mục cần bỏ qua
            int skip = (currentPage - 1) * PageSize;

            // Lấy danh sách media phân trang từ tầng dữ liệu
            var pagedMedias = await _mediaRepository.GetPagedMediasAsync(medias, skip, PageSize);

            // Trả về tuple chứa danh sách media, tổng số trang, và trang hiện tại
            return (pagedMedias, pageNumbers, currentPage);
        }

        // Lấy danh sách media phân trang với bộ lọc theo thể loại và từ khóa cho MoviesFilter
        public async Task<(List<MediaModel> Medias, int PageNumbers, int CurrentPage)> GetFilteredPagedMediasAsync(int? genreId, string? keyword, int? pageIndex) {
            // Lấy danh sách media dưới dạng IQueryable từ tầng dữ liệu
            var medias = _mediaRepository.GetMediasQueryable();

            // Áp dụng bộ lọc theo thể loại nếu có
            if (genreId.HasValue)
                medias = medias.Where(m => m.Genres.Any(g => g.GenreId == genreId.Value));

            // Áp dụng bộ lọc theo từ khóa nếu có
            if (!string.IsNullOrEmpty(keyword))
                medias = medias.Where(m => m.MediaName.Contains(keyword));

            // Tính tổng số media và số trang
            int totalMedia = await _mediaRepository.CountMediasAsync(medias);
            int pageNumbers = (int)Math.Ceiling(totalMedia / (float)PageSize);

            // Đảm bảo page không nhỏ hơn 1
            int currentPage = pageIndex == null || pageIndex <= 0 ? 1 : pageIndex.Value;

            // Tính số lượng mục cần bỏ qua
            int skip = (currentPage - 1) * PageSize;

            // Lấy danh sách media phân trang từ tầng dữ liệu
            var pagedMedias = await _mediaRepository.GetPagedMediasAsync(medias, skip, PageSize);

            // Trả về tuple chứa danh sách media, tổng số trang, và trang hiện tại
            return (pagedMedias, pageNumbers, currentPage);
        }
    }
}