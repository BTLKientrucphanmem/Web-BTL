using Microsoft.AspNetCore.Mvc.Rendering;
using Web_BTL.DataAccessLayer.Models;
using Web_BTL.DataAccessLayer.Repository;

namespace Web_BTL.BusinessLogicLayer.Services {

    public class GenreService : IGenreService {
        private readonly IGenreRepository _genreRepository;

        public GenreService(IGenreRepository genreRepository) {
            _genreRepository = genreRepository;
        }

        public async Task<(List<MediaModel> Medias, List<SelectListItem> Actors, List<SelectListItem> Genres, List<string> Qualities)> GetFilteredMediasAsync(int? actorId, int? genreId, string quality, string duration) {
            var medias = await _genreRepository.GetMediasAsync();

            if (actorId.HasValue)
                medias = medias.Intersect(await _genreRepository.GetMediasByActorIdAsync(actorId.Value)).ToList();

            if (genreId.HasValue)
                medias = medias.Intersect(await _genreRepository.GetMediasByGenreIdAsync(genreId.Value)).ToList();

            if (!string.IsNullOrEmpty(quality))
                medias = medias.Intersect(await _genreRepository.GetMediasByQualityAsync(quality)).ToList();

            if (!string.IsNullOrEmpty(duration))
                medias = medias.Intersect(await _genreRepository.GetMediasByDurationAsync(duration)).ToList();

            var actors = await _genreRepository.GetActorSelectListAsync();
            var genres = await _genreRepository.GetGenreSelectListAsync();
            var qualities = await _genreRepository.GetQualityListAsync();

            return (medias, actors, genres, qualities);
        }

        public async Task<(List<MediaModel> Medias, int PageNumbers, int CurrentPage, string SearchTerm)> GetPagedMediasAsync(int? pageIndex, string searchTerm) {
            var medias = await _genreRepository.GetMediasAsync();
            if (!string.IsNullOrEmpty(searchTerm))
                medias = medias.Intersect(await _genreRepository.GetMediasBySearchTermAsync(searchTerm)).ToList();

            int pageSize = 8;
            int totalItems = medias.Count;
            int pageNumbers = (int)Math.Ceiling(totalItems / (double)pageSize);
            int currentPage = pageIndex ?? 1;

            var pagedMedias = medias
                .Skip((currentPage - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return (pagedMedias, pageNumbers, currentPage, searchTerm);
        }

        public async Task<List<MediaModel>> GetMediasByGenreIdAsync(int genreId, int pageSize = 8) {
            var medias = await _genreRepository.GetMediasByGenreIdAsync(genreId);
            return medias.Take(pageSize).ToList();
        }
    }
}

