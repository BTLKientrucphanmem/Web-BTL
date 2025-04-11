using Microsoft.AspNetCore.Mvc.Rendering;
using Web_BTL.DataAccessLayer.Models;


namespace Web_BTL.BusinessLogicLayer.Services {
    public interface IAdminService {
        Task<List<MediaModel>> GetAllMediasAsync();
        Task<(bool Success, string ErrorMessage, MediaModel? Media)> AddMediaAsync(MediaModel media, IFormFile? image, IFormFile? banner, IFormFile? video, List<int> selectedGenreIds);
        Task<(bool Success, MediaModel? Media)> GetMediaByIdAsync(int mediaId);
        Task<(bool Success, string ErrorMessage, MediaModel? Media)> EditMediaAsync(int mediaId, MediaModel model, IFormFile? image, IFormFile? banner, IFormFile? video, List<int> selectedGenreIds, List<int> selectedActorIds, List<int> selectedActorMainIds);
        Task<bool> DeleteMediaAsync(int mediaId);
        Task<List<GenreModel>> GetAllGenresAsync();
        Task<bool> AddGenreAsync(GenreModel genre);
        Task<bool> EditGenreAsync(GenreModel genre);
        Task<bool> DeleteGenreAsync(int genreId);
        Task<List<ActorModel>> GetAllActorsAsync();
        Task<bool> AddActorAsync(ActorModel actor);
        Task<bool> EditActorAsync(ActorModel actor);
        Task<bool> DeleteActorAsync(int actorId);
        Task<List<CustomerModel>> GetAllCustomersAsync();
        Task<(bool Success, bool NewState)> ToggleCustomerStateAsync(int customerId);
        Task<List<MediaModel>> GetMediasByGenreAsync(int genreId);
        List<SelectListItem> GetGenreSelectList();
        List<SelectListItem> GetActorSelectList();
    }
}