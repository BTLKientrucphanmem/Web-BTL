using Microsoft.EntityFrameworkCore;
using Web_BTL.DataAccessLayer.Models;



namespace Web_BTL.DataAccessLayer.Repository {
    public class AdminRepository : IAdminRepository {
        private readonly DBXemPhimContext _datacontext;

        public AdminRepository(DBXemPhimContext datacontext) {
            _datacontext = datacontext;
        }

        public async Task<List<MediaModel>> GetAllMediasAsync() {
            return await _datacontext.Medias.ToListAsync();
        }

        public async Task<MediaModel?> GetMediaByIdAsync(int mediaId) {
            return await _datacontext.Medias.FirstOrDefaultAsync(m => m.MediaId == mediaId);
        }

        public async Task AddMediaAsync(MediaModel media) {
            await _datacontext.Medias.AddAsync(media);
            await _datacontext.SaveChangesAsync();
        }

        public async Task UpdateMediaAsync(MediaModel media) {
            _datacontext.Medias.Update(media);
            await _datacontext.SaveChangesAsync();
        }

        public async Task DeleteMediaAsync(int mediaId) {
            var media = await GetMediaByIdAsync(mediaId);
            if (media != null) {
                _datacontext.Medias.Remove(media);
                await _datacontext.SaveChangesAsync();
            }
        }

        public async Task<List<GenreModel>> GetAllGenresAsync() {
            return await _datacontext.Genres.ToListAsync();
        }

        public async Task<GenreModel?> GetGenreByIdAsync(int genreId) {
            return await _datacontext.Genres.FirstOrDefaultAsync(g => g.GenreId == genreId);
        }

        public async Task AddGenreAsync(GenreModel genre) {
            await _datacontext.Genres.AddAsync(genre);
            await _datacontext.SaveChangesAsync();
        }

        public async Task UpdateGenreAsync(GenreModel genre) {
            _datacontext.Genres.Update(genre);
            await _datacontext.SaveChangesAsync();
        }

        public async Task DeleteGenreAsync(int genreId) {
            var genre = await GetGenreByIdAsync(genreId);
            if (genre != null) {
                _datacontext.Genres.Remove(genre);
                await _datacontext.SaveChangesAsync();
            }
        }

        public async Task<List<ActorModel>> GetAllActorsAsync() {
            return await _datacontext.Actors.ToListAsync();
        }

        public async Task<ActorModel?> GetActorByIdAsync(int actorId) {
            return await _datacontext.Actors.FindAsync(actorId);
        }

        public async Task AddActorAsync(ActorModel actor) {
            await _datacontext.Actors.AddAsync(actor);
            await _datacontext.SaveChangesAsync();
        }

        public async Task UpdateActorAsync(ActorModel actor) {
            _datacontext.Actors.Update(actor);
            await _datacontext.SaveChangesAsync();
        }

        public async Task DeleteActorAsync(int actorId) {
            var actor = await GetActorByIdAsync(actorId);
            if (actor != null) {
                _datacontext.Actors.Remove(actor);
                await _datacontext.SaveChangesAsync();
            }
        }

        public async Task<List<CustomerModel>> GetAllCustomersAsync() {
            return await _datacontext.Customers.ToListAsync();
        }

        public async Task<CustomerModel?> GetCustomerByIdAsync(int customerId) {
            return await _datacontext.Customers.FindAsync(customerId);
        }

        public async Task UpdateCustomerAsync(CustomerModel customer) {
            _datacontext.Customers.Update(customer);
            await _datacontext.SaveChangesAsync();
        }

        public async Task<List<MediaModel>> GetMediasByGenreAsync(int genreId) {
            return await _datacontext.Medias
                .Where(m => m.Genres.Any(g => g.GenreId == genreId))
                .ToListAsync();
        }

        public async Task<List<Actor_MediaModel>> GetActorMediasByMediaIdAsync(int mediaId) {
            return await _datacontext.Actor_Medias
                .Where(am => am.MediaId == mediaId)
                .ToListAsync();
        }

        public async Task AddActorMediaAsync(Actor_MediaModel actorMedia) {
            await _datacontext.Actor_Medias.AddAsync(actorMedia);
            await _datacontext.SaveChangesAsync();
        }

        public async Task DeleteActorMediasByMediaIdAsync(int mediaId) {
            var actorMedias = await GetActorMediasByMediaIdAsync(mediaId);
            _datacontext.Actor_Medias.RemoveRange(actorMedias);
            await _datacontext.SaveChangesAsync();
        }
    }
}