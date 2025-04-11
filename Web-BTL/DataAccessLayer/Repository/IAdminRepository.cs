
using Web_BTL.DataAccessLayer.Models;

namespace Web_BTL.DataAccessLayer.Repository {
    public interface IAdminRepository {
        Task<List<MediaModel>> GetAllMediasAsync();
        Task<MediaModel?> GetMediaByIdAsync(int mediaId);
        Task AddMediaAsync(MediaModel media);
        Task UpdateMediaAsync(MediaModel media);
        Task DeleteMediaAsync(int mediaId);
        Task<List<GenreModel>> GetAllGenresAsync();
        Task<GenreModel?> GetGenreByIdAsync(int genreId);
        Task AddGenreAsync(GenreModel genre);
        Task UpdateGenreAsync(GenreModel genre);
        Task DeleteGenreAsync(int genreId);
        Task<List<ActorModel>> GetAllActorsAsync();
        Task<ActorModel?> GetActorByIdAsync(int actorId);
        Task AddActorAsync(ActorModel actor);
        Task UpdateActorAsync(ActorModel actor);
        Task DeleteActorAsync(int actorId);
        Task<List<CustomerModel>> GetAllCustomersAsync();
        Task<CustomerModel?> GetCustomerByIdAsync(int customerId);
        Task UpdateCustomerAsync(CustomerModel customer);
        Task<List<MediaModel>> GetMediasByGenreAsync(int genreId);
        Task<List<Actor_MediaModel>> GetActorMediasByMediaIdAsync(int mediaId);
        Task AddActorMediaAsync(Actor_MediaModel actorMedia);
        Task DeleteActorMediasByMediaIdAsync(int mediaId);
    }
}