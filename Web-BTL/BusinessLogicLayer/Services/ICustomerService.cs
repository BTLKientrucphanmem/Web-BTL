using Web_BTL.DataAccessLayer.Models;


namespace Web_BTL.BusinessLogicLayer.Services {
    public interface ICustomerService {
        Task<(bool Success, List<MediaModel> FavoriteMedias)> GetFavoriteMediasAsync();
        Task<(bool Success, List<MediaModel> WatchedMedias)> GetWatchedMediasAsync();
        bool ValidateCustomer();
    }
}