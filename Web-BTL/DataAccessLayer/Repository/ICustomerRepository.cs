using Web_BTL.DataAccessLayer.Models;


namespace Web_BTL.DataAccessLayer.Repository {
    public interface ICustomerRepository {
        Task<CustomerModel?> GetCustomerByEmailAsync(string email);
        Task<List<MediaModel>> GetFavoriteMediasAsync(int watchListId);
        Task<List<MediaModel>> GetWatchedMediasAsync(int watchListId);
    }
}