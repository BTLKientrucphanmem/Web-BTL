
using Web_BTL.DataAccessLayer.Models;
using Web_BTL.DataAccessLayer.Repository;


namespace Web_BTL.BusinessLogicLayer.Services {
    // Lớp triển khai logic nghiệp vụ cho trang chủ
    public class HomeService : IHomeService {
        private readonly IHomeRepository _homeRepository; // Repository truy cập dữ liệu

        // Constructor: Tiêm IHomeRepository qua Dependency Injection
        public HomeService(IHomeRepository homeRepository) {
            _homeRepository = homeRepository;
        }

        // Lấy dữ liệu cho trang Index (tất cả media và danh sách theo loại)
        public async Task<(List<MediaModel> AllMedias, List<MediaModel> Cartoons, List<MediaModel> Movies, List<MediaModel> Series)> GetHomeDataAsync() {
            // Lấy tất cả media từ tầng dữ liệu
            var allMedias = await _homeRepository.GetAllMediasAsync();

            // Lấy danh sách media theo từng loại thể loại
            var cartoons = await _homeRepository.GetMediasByGenreTypeAsync("Cartoon");
            var movies = await _homeRepository.GetMediasByGenreTypeAsync("Movie");
            var series = await _homeRepository.GetMediasByGenreTypeAsync("Series");

            // Trả về tuple chứa tất cả danh sách media cần thiết
            return (allMedias, cartoons, movies, series);
        }

        // Lấy danh sách media theo ID thể loại cho GenreFilter
        public async Task<List<MediaModel>> GetMediasByGenreIdAsync(int genreId) {
            // Gọi tầng dữ liệu để lấy danh sách media theo ID thể loại
            return await _homeRepository.GetMediasByGenreIdAsync(genreId);
        }

        // Lấy danh sách media hoạt hình cho CartoonMovieFilter
        public async Task<List<MediaModel>> GetCartoonMediasAsync() {
            // Gọi tầng dữ liệu để lấy danh sách media thuộc loại "Cartoon"
            return await _homeRepository.GetMediasByGenreTypeAsync("Cartoon");
        }
    }
}