using Microsoft.EntityFrameworkCore;
using Web_BTL.DataAccessLayer.Models;




namespace Web_BTL.DataAccessLayer.Repository {
    public class CustomerRepository : ICustomerRepository {
        private readonly DBXemPhimContext _dataContext;

        public CustomerRepository(DBXemPhimContext dataContext) {
            _dataContext = dataContext;
        }

        // Lấy thông tin khách hàng theo email
        public async Task<CustomerModel?> GetCustomerByEmailAsync(string email) {
            return await _dataContext.Customers
                .FirstOrDefaultAsync(c => c.UserEmail == email);
        }

        // Lấy danh sách media yêu thích của khách hàng
        public async Task<List<MediaModel>> GetFavoriteMediasAsync(int watchListId) {
            return await (from lm in _dataContext.ListMedia
                          join m in _dataContext.Medias on lm.MediaId equals m.MediaId
                          where lm.WatchListId == watchListId && lm.Favorite == true
                          select m).ToListAsync();
        }

        // Lấy danh sách media đã xem của khách hàng
        public async Task<List<MediaModel>> GetWatchedMediasAsync(int watchListId) {
            return await (from lm in _dataContext.ListMedia
                          join m in _dataContext.Medias on lm.MediaId equals m.MediaId
                          where lm.WatchListId == watchListId && lm.IsWatched == true
                          select m).ToListAsync();
        }
    }
}