using Microsoft.EntityFrameworkCore;
using Web_BTL.DataAccessLayer.Models;

namespace Web_BTL.DataAccessLayer {
    public class BornData_test {
        public static void SeedingData(DBXemPhimContext _context) {
            /*
                Migrate dùng để xem các file nào đã áp dụng migrate 
                những migrate nào đã áp dụng thì sẽ bỏ qua
                và những migrate nào chưa được áp dụng thì sẽ tự động dịch và áp dụng
            ==> giúp đồng bộ hoá dữ liệu
             */
            _context.Database.Migrate();
            if (!_context.Admins.Any()) // seed data table Admin
            /*
            UserId, UserName, UserLogin, LoginPassword, UserEmail,
            UserCreateDate, UserImagePath, UserState, UserDuration, _Role
             */
            {
                _context.Admins.AddRange(
                    new AdminModel {
                        UserName = "Bui Le Quang Hai",
                        UserLogin = "haibui,",
                        LoginPassword = "12345678@Aa",
                        UserEmail = "haibui.com",
                        UserCreateDate = new DateTime(2024, 8, 12),
                        UserImagePath = "default.jpg",
                        UserState = true,
                        UserDuration = new TimeSpan(0, 0, 0),
                        Role = Role.SuperAdmin
                    }
                    );
                _context.SaveChanges();
            }
            if (!_context.WatchLists.Any()) {
                _context.WatchLists.AddRange(
                    new WatchListModel { CustomerId = 1 },
                    new WatchListModel { CustomerId = 2 },
                    new WatchListModel { CustomerId = 3 }
                    );
                _context.SaveChanges();
            }
            if (!_context.Customers.Any())
            /*
            UserId, UserName, UserLogin, LoginPassword, UserEmail,
            UserCreateDate, UserImagePath, UserState, UserDuration, ServicePackage
            */
            {
                _context.Customers.AddRange(
                    new CustomerModel {
                        UserName = "Bui Le Quang Hai",
                        UserLogin = "utcno1",
                        LoginPassword = "12345678@Aa",
                        UserEmail = "utc28@gmail.com",
                        UserCreateDate = new DateTime(2024, 8, 12),
                        UserImagePath = "default.jpg",
                        UserState = true,
                        UserDuration = new TimeSpan(0, 0, 0),
                        WatchListId = 1,
                    },
                    new CustomerModel {
                        UserName = "Bui Le Quang Hai",
                        UserLogin = "utcno2",
                        LoginPassword = "12345678@Aa",
                        UserEmail = "utc2004@gmail.com",
                        UserCreateDate = new DateTime(2024, 8, 12),
                        UserImagePath = "default.jpg",
                        UserState = true,
                        UserDuration = new TimeSpan(0, 0, 0),
                        WatchListId = 2,
                    }

                    );
                _context.SaveChanges();
            }
            if (!_context.Medias.Any())
            /*
            MediaName, MediaGenre, MediaUrl, MediaDescription, MediaQuality, 
            ReleaseDate, MediaAgeRating, MediaImagePath, MediaDuration, MediaState
            */
            {
                _context.Medias.AddRange(
                    new MediaModel {
                        MediaName = "Supergirl",
                        MediaUrl = "supergirl",
                        MediaDescription = "The film is very good",
                        MediaQuality = "HD",
                        ReleaseDate = new DateTime(2024, 9, 12),
                        MediaAgeRating = 16,
                        MediaImagePath = "supergirl",
                        MediaDuration = new TimeSpan(2, 0, 0),
                    },
                    new MediaModel {
                        MediaName = "Transformer",
                        MediaUrl = "transformer",
                        MediaDescription = "The film is the best",
                        MediaQuality = "HD",
                        ReleaseDate = new DateTime(2024, 9, 12),
                        MediaAgeRating = 16,
                        MediaImagePath = "transformer",
                        MediaDuration = new TimeSpan(2, 0, 0),
                    },
                    new MediaModel {
                        MediaName = "Demon Slayer",
                        MediaUrl = "demonslayer",
                        MediaDescription = "The cartoon is very good",
                        MediaQuality = "HD",
                        ReleaseDate = new DateTime(2024, 9, 12),
                        MediaAgeRating = 16,
                        MediaImagePath = "demonslayer",
                        MediaDuration = new TimeSpan(2, 0, 0),
                    }
                    );
                _context.SaveChanges();
            }
            if (!_context.Reviews.Any()) {
                _context.Reviews.AddRange(
                    new ReviewModel { ReviewContent = "Very very good", ReviewRating = 5.0, ReviewCreateDate = new DateTime(2024, 9, 12), CustomerId = 1, MediaId = 1 },
                    new ReviewModel { ReviewContent = "Very good", ReviewRating = 5.0, ReviewCreateDate = new DateTime(2024, 9, 12), CustomerId = 1, MediaId = 2 },
                    new ReviewModel { ReviewContent = "Very very very good", ReviewRating = 5.0, ReviewCreateDate = new DateTime(2024, 9, 12), CustomerId = 1, MediaId = 3 }
                    );
                _context.SaveChanges();
            }
            if (!_context.Genres.Any()) {
                _context.Genres.AddRange(
                    new GenreModel { Type = "Movie" },
                    new GenreModel { Type = "Cartoon" },
                    new GenreModel { Type = "Series" }
                    );
                _context.SaveChanges();
            }
            if (!_context.Actors.Any()) {
                _context.Actors.AddRange(
                    new ActorModel { ActorName = "Megan Fox", AcctorDate = new DateTime(1986, 5, 16) }, //16 tháng 5, 1986 (38 tuổi)
                    new ActorModel { ActorName = "Shia LaBeouf", AcctorDate = new DateTime(1986, 6, 11) }, // 11 tháng 6, 1986 (38 tuổi)
                    new ActorModel { ActorName = "Rachael Taylor", AcctorDate = new DateTime(1984, 7, 11) } // 11 tháng 7, 1984 (40 tuổi)
                    );
                _context.SaveChanges();
            }
            //chèn các giá trị cho bang phụ khi chèn xong thì xoá đi
            // media
            var media1 = _context.Medias.FirstOrDefault(m => m.MediaId == 1);
            var media2 = _context.Medias.FirstOrDefault(m => m.MediaId == 2);
            var media3 = _context.Medias.FirstOrDefault(m => m.MediaId == 3);

            if (!_context.ListMedia.Any()) {
                var watchList1 = _context.WatchLists.FirstOrDefault(w => w.CustomerId == 1);
                var watchList2 = _context.WatchLists.FirstOrDefault(w => w.CustomerId == 2);
                var watchList3 = _context.WatchLists.FirstOrDefault(w => w.CustomerId == 3);
                _context.ListMedia.AddRange(
                    new ListMediaModel {
                        WatchListId = watchList1.WatchListId,
                        MediaId = media1.MediaId,
                        IsWatched = false,
                        Favorite = false,
                        AddDate = DateTime.Now
                    },
                    new ListMediaModel {
                        WatchListId = watchList2.WatchListId,
                        MediaId = media2.MediaId,
                        IsWatched = false,
                        Favorite = false,
                        AddDate = DateTime.Now
                    },
                    new ListMediaModel {
                        WatchListId = watchList3.WatchListId,
                        MediaId = media3.MediaId,
                        IsWatched = false,
                        Favorite = false,
                        AddDate = DateTime.Now
                    },
                    new ListMediaModel {
                        WatchListId = watchList2.WatchListId,
                        MediaId = media3.MediaId,
                        IsWatched = false,
                        Favorite = false,
                        AddDate = DateTime.Now
                    },
                    new ListMediaModel {
                        WatchListId = watchList2.WatchListId,
                        MediaId = media1.MediaId,
                        IsWatched = false,
                        Favorite = false,
                        AddDate = DateTime.Now
                    }
                    );
                _context.SaveChanges();
            }
        }
    }
}