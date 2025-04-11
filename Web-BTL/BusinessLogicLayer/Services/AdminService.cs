
using Microsoft.AspNetCore.Mvc.Rendering;
using Web_BTL.DataAccessLayer.Models;
using Web_BTL.DataAccessLayer.Repository;

namespace Web_BTL.BusinessLogicLayer.Services {
    public class AdminService : IAdminService {
        private readonly IAdminRepository _adminRepository;
        private readonly IWebHostEnvironment _environment;
        private readonly SaveImageVideo _save;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AdminService(IAdminRepository adminRepository, IWebHostEnvironment environment, SaveImageVideo save, IHttpContextAccessor httpContextAccessor) {
            _adminRepository = adminRepository;
            _environment = environment;
            _save = save;
            _httpContextAccessor = httpContextAccessor;
        }

        private bool HasPermission(string requiredRole) {
            var role = _httpContextAccessor.HttpContext?.Session.GetString("Admin");
            return role != null && (role == Role.SuperAdmin.ToString() || role == requiredRole);
        }

        public async Task<List<MediaModel>> GetAllMediasAsync() {
            return await _adminRepository.GetAllMediasAsync();
        }

        public async Task<(bool Success, string ErrorMessage, MediaModel? Media)> AddMediaAsync(MediaModel media, IFormFile? image, IFormFile? banner, IFormFile? video, List<int> selectedGenreIds) {
            if (!HasPermission(Role.Movie_Management.ToString()))
                return (false, "Quyền hạn của bạn không đủ", null);

            if (selectedGenreIds.Count == 0)
                return (false, "Vui lòng chọn ít nhất một thể loại", media);

            if (image != null && image.Length > 0)
                media.MediaImagePath = await _save.SaveImageAsync(_environment, "images/medias", "", media.MediaName, image);
            if (banner != null && banner.Length > 0)
                media.MediaBannerPath = await _save.SaveImageAsync(_environment, "images/banners", "", media.MediaName + "banner", banner);
            if (video != null && video.Length > 0) {
                var result = await _save.SaveVideoAsync(_environment, "videos", "", media.MediaName + media.MediaQuality, video, true);
                media.MediaUrl = result.videoName;
                media.MediaDuration = result.duration;
            }

            foreach (var genreId in selectedGenreIds) {
                var genre = await _adminRepository.GetGenreByIdAsync(genreId);
                if (genre != null) media.Genres.Add(genre);
            }

            await _adminRepository.AddMediaAsync(media);
            return (true, null, media);
        }

        public async Task<(bool Success, MediaModel? Media)> GetMediaByIdAsync(int mediaId) {
            if (!HasPermission(Role.Movie_Management.ToString()))
                return (false, null);

            var media = await _adminRepository.GetMediaByIdAsync(mediaId);
            return (media != null, media);
        }

        public async Task<(bool Success, string ErrorMessage, MediaModel? Media)> EditMediaAsync(int mediaId, MediaModel model, IFormFile? image, IFormFile? banner, IFormFile? video, List<int> selectedGenreIds, List<int> selectedActorIds, List<int> selectedActorMainIds) {
            if (!HasPermission(Role.Movie_Management.ToString()))
                return (false, "Quyền hạn của bạn không đủ", null);

            var media = await _adminRepository.GetMediaByIdAsync(mediaId);
            if (media == null || mediaId != model.MediaId)
                return (false, "Không tìm thấy media", null);

            media.MediaName = model.MediaName;
            media.MediaDescription = model.MediaDescription;
            media.MediaQuality = model.MediaQuality;
            media.ReleaseDate = model.ReleaseDate;
            media.MediaAgeRating = model.MediaAgeRating;

            if (image != null && image.Length > 0)
                media.MediaImagePath = await _save.SaveImageAsync(_environment, "images/medias", "", media.MediaName, image);
            if (banner != null && banner.Length > 0)
                media.MediaBannerPath = await _save.SaveImageAsync(_environment, "images/banners", "", media.MediaName + "banner", banner);
            if (video != null && video.Length > 0) {
                var result = await _save.SaveVideoAsync(_environment, "videos", "", media.MediaName + media.MediaQuality, video, true);
                media.MediaUrl = result.videoName;
                media.MediaDuration = result.duration;
            }

            if (selectedGenreIds.Count > 0) {
                media.Genres.Clear();
                foreach (var genreId in selectedGenreIds) {
                    var genre = await _adminRepository.GetGenreByIdAsync(genreId);
                    if (genre != null) media.Genres.Add(genre);
                }
            }

            if (selectedActorIds.Count > 0) {
                await _adminRepository.DeleteActorMediasByMediaIdAsync(mediaId);
                foreach (var actorId in selectedActorIds) {
                    await _adminRepository.AddActorMediaAsync(new Actor_MediaModel {
                        MediaId = mediaId,
                        ActorId = actorId,
                        IsMain = selectedActorMainIds.Contains(actorId)
                    });
                }
            }

            await _adminRepository.UpdateMediaAsync(media);
            return (true, null, media);
        }

        public async Task<bool> DeleteMediaAsync(int mediaId) {
            if (!HasPermission(Role.Movie_Management.ToString()))
                return false;

            var media = await _adminRepository.GetMediaByIdAsync(mediaId);
            if (media != null) {
                _save.DeleteFile(_environment, "images/medias", media.MediaImagePath);
                _save.DeleteFile(_environment, "videos", media.MediaUrl);
                await _adminRepository.DeleteMediaAsync(mediaId);
                return true;
            }
            return false;
        }

        public async Task<List<GenreModel>> GetAllGenresAsync() {
            return await _adminRepository.GetAllGenresAsync();
        }

        public async Task<bool> AddGenreAsync(GenreModel genre) {
            if (!HasPermission(Role.Movie_Management.ToString()))
                return false;

            await _adminRepository.AddGenreAsync(genre);
            return true;
        }

        public async Task<bool> EditGenreAsync(GenreModel genre) {
            if (!HasPermission(Role.Movie_Management.ToString()))
                return false;

            var existingGenre = await _adminRepository.GetGenreByIdAsync(genre.GenreId);
            if (existingGenre != null) {
                existingGenre.Type = genre.Type;
                await _adminRepository.UpdateGenreAsync(existingGenre);
                return true;
            }
            return false;
        }

        public async Task<bool> DeleteGenreAsync(int genreId) {
            if (!HasPermission(Role.Movie_Management.ToString()))
                return false;

            await _adminRepository.DeleteGenreAsync(genreId);
            return true;
        }

        public async Task<List<ActorModel>> GetAllActorsAsync() {
            return await _adminRepository.GetAllActorsAsync();
        }

        public async Task<bool> AddActorAsync(ActorModel actor) {
            if (!HasPermission(Role.Movie_Management.ToString()))
                return false;

            await _adminRepository.AddActorAsync(actor);
            return true;
        }

        public async Task<bool> EditActorAsync(ActorModel actor) {
            if (!HasPermission(Role.Movie_Management.ToString()))
                return false;

            var existingActor = await _adminRepository.GetActorByIdAsync(actor.ActorID);
            if (existingActor != null) {
                existingActor.ActorName = actor.ActorName;
                existingActor.AcctorDate = actor.AcctorDate;
                await _adminRepository.UpdateActorAsync(existingActor);
                return true;
            }
            return false;
        }

        public async Task<bool> DeleteActorAsync(int actorId) {
            if (!HasPermission(Role.Movie_Management.ToString()))
                return false;

            await _adminRepository.DeleteActorAsync(actorId);
            return true;
        }

        public async Task<List<CustomerModel>> GetAllCustomersAsync() {
            return await _adminRepository.GetAllCustomersAsync();
        }

        public async Task<(bool Success, bool NewState)> ToggleCustomerStateAsync(int customerId) {
            if (!HasPermission(Role.ServicePackage_Management.ToString()))
                return (false, false);

            var customer = await _adminRepository.GetCustomerByIdAsync(customerId);
            if (customer != null) {
                customer.UserState = !customer.UserState;
                await _adminRepository.UpdateCustomerAsync(customer);
                return (true, customer.UserState.Value);
            }
            return (false, false);
        }

        public async Task<List<MediaModel>> GetMediasByGenreAsync(int genreId) {
            return await _adminRepository.GetMediasByGenreAsync(genreId);
        }

        public List<SelectListItem> GetGenreSelectList() {
            var genres = _adminRepository.GetAllGenresAsync().Result;
            return genres.Select(g => new SelectListItem {
                Text = g.Type,
                Value = g.GenreId.ToString()
            }).ToList();
        }

        public List<SelectListItem> GetActorSelectList() {
            var actors = _adminRepository.GetAllActorsAsync().Result;
            return actors.Select(a => new SelectListItem {
                Text = a.ActorName,
                Value = a.ActorID.ToString()
            }).ToList();
        }
    }
}