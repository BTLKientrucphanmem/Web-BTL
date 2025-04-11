using Web_BTL.DataAccessLayer.Models;
using Web_BTL.DataAccessLayer.Repository;

namespace Web_BTL.BusinessLogicLayer.Services {
    // Lớp triển khai logic nghiệp vụ liên quan đến phim
    public class MovieService : IMovieService {
        private readonly IMovieRepository _movieRepository; // Repository truy cập dữ liệu

        // Constructor: Tiêm IMovieRepository qua Dependency Injection
        public MovieService(IMovieRepository movieRepository) {
            _movieRepository = movieRepository;
        }

        // Lấy thông tin media và thêm vào danh sách xem nếu chưa có (cho Index)
        public async Task<MediaModel?> GetMediaAndUpdateWatchListAsync(int movieId, string? userEmail) {
            // Lấy thông tin media từ tầng dữ liệu
            var movie = await _movieRepository.GetMediaByIdAsync(movieId);
            if (movie == null)
                return null; // Trả về null nếu không tìm thấy media

            // Nếu người dùng đã đăng nhập, xử lý danh sách xem
            if (!string.IsNullOrEmpty(userEmail)) {
                var customer = await _movieRepository.GetCustomerByEmailAsync(userEmail);
                if (customer != null && customer.WatchListId.HasValue) {
                    // Kiểm tra xem media đã có trong danh sách xem chưa
                    bool isInList = await _movieRepository.IsMediaInWatchListAsync(customer.WatchListId.Value, movieId);
                    if (!isInList) {
                        // Tạo bản ghi mới cho danh sách xem
                        var newListMedia = new ListMediaModel {
                            WatchListId = customer.WatchListId.Value,
                            MediaId = movieId,
                            IsWatched = false,
                            Favorite = false,
                            AddDate = DateTime.Now
                        };

                        // Thêm vào danh sách xem
                        await _movieRepository.AddMediaToWatchListAsync(newListMedia);
                    }
                }
            }

            // Trả về thông tin media
            return movie;
        }

        // Lấy thông tin media và cập nhật trạng thái đã xem (cho Watching)
        public async Task<MediaModel?> GetMediaAndMarkAsWatchedAsync(int movieId, string? userEmail) {
            // Lấy thông tin media từ tầng dữ liệu
            var movie = await _movieRepository.GetMediaByIdAsync(movieId);
            if (movie == null)
                return null; // Trả về null nếu không tìm thấy media

            // Nếu người dùng đã đăng nhập, cập nhật trạng thái đã xem
            if (!string.IsNullOrEmpty(userEmail)) {
                var customer = await _movieRepository.GetCustomerByEmailAsync(userEmail);
                if (customer != null && customer.WatchListId.HasValue) {
                    // Lấy bản ghi ListMedia từ tầng dữ liệu
                    var listMedia = await _movieRepository.GetListMediaAsync(customer.WatchListId.Value, movieId);
                    if (listMedia != null) {
                        // Cập nhật trạng thái đã xem
                        listMedia.IsWatched = true;
                        await _movieRepository.UpdateListMediaAsync(listMedia);
                    }
                }
            }

            // Trả về thông tin media
            return movie;
        }

        // Thêm media vào danh sách yêu thích (cho AddFavourite)
        public async Task<(bool Success, bool IsRedirectToLogin)> AddFavouriteAsync(int mediaId, string? userEmail) {
            // Kiểm tra xem người dùng đã đăng nhập chưa
            if (string.IsNullOrEmpty(userEmail))
                return (false, true); // Chưa đăng nhập, yêu cầu chuyển hướng đến trang đăng nhập

            // Lấy thông tin khách hàng từ tầng dữ liệu
            var customer = await _movieRepository.GetCustomerByEmailAsync(userEmail);
            if (customer == null || !customer.WatchListId.HasValue)
                return (false, true); // Không tìm thấy khách hàng, yêu cầu đăng nhập

            // Lấy bản ghi ListMedia từ tầng dữ liệu
            var listMedia = await _movieRepository.GetListMediaAsync(customer.WatchListId.Value, mediaId);
            if (listMedia == null)
                return (false, false); // Không tìm thấy bản ghi, trả về thất bại

            // Cập nhật trạng thái yêu thích
            listMedia.Favorite = true;
            await _movieRepository.UpdateListMediaAsync(listMedia);

            // Trả về thành công và không cần chuyển hướng
            return (true, false);
        }
    }
}