using Microsoft.EntityFrameworkCore;
using Web_BTL.DataAccessLayer.Models;

namespace Web_BTL.DataAccessLayer {
    public class DBXemPhimContext : DbContext {
        public DBXemPhimContext(DbContextOptions<DBXemPhimContext> options) : base(options) { }
        // Bảng phim và thể loại
        public DbSet<MediaModel> Medias { get; set; }
        public DbSet<GenreModel> Genres { get; set; }

        // Bảng review(đánh giá)
        public DbSet<ReviewModel> Reviews { get; set; }

        // Bảng Admin và Customer
        public DbSet<AdminModel> Admins { get; set; }
        public DbSet<CustomerModel> Customers { get; set; }

        // Bảng Actor
        public DbSet<ActorModel> Actors { get; set; }
        public DbSet<Actor_MediaModel> Actor_Medias { get; set; }

        // Bảng Watch List và bảng phụ
        public DbSet<WatchListModel> WatchLists { get; set; }
        // Bảng phụ giữa media và watchlist
        public DbSet<ListMediaModel> ListMedia { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            // thiết lập bảng phụ cho Media và Actor
            modelBuilder.Entity<Actor_MediaModel>().
                HasKey(am => new { am.MediaId, am.ActorId });
            modelBuilder.Entity<Actor_MediaModel>().
                HasOne(m => m.Media).WithMany(a => a.Actors).
                HasForeignKey(m => m.MediaId).
                OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Actor_MediaModel>().
                HasOne(a => a.Actor).WithMany(m => m.Medias).
                HasForeignKey(a => a.ActorId).
                OnDelete(DeleteBehavior.Cascade);
            // thiết lập bảng phụ cho Media và Genres
            modelBuilder.Entity<MediaModel>().
                HasMany(m => m.Genres).
                WithMany(g => g.Medias).
                UsingEntity<Dictionary<string, object>>(
                    "Media_Genre",
                    j => j.HasOne<GenreModel>().WithMany().HasForeignKey("GenreId").OnDelete(DeleteBehavior.Cascade),
                    j => j.HasOne<MediaModel>().WithMany().HasForeignKey("MediaId").OnDelete(DeleteBehavior.Cascade)
                );
            modelBuilder.Entity<AdminModel>().
                Property(a => a.Role).
                HasConversion<string>();  // Ánh xạ enum thành chuỗi

            // cấu hình cho bảng phụ có đủ điều kiện để kết nối với 2 bảng còn lại(Media và WatchList) - tạo 2 khoá cho bảng
            modelBuilder.Entity<ListMediaModel>().
                HasKey(lm => new { lm.WatchListId, lm.MediaId });
            // thiết lập mối quan hệ giữa bảng ListMediaModel và WatchListModel, với WatchListModel có nhiều ListMediaModel(nhiều media thuộc về một danh sách xem).
            modelBuilder.Entity<ListMediaModel>().
                HasOne(lm => lm.watchList).
                WithMany(w => w.Medias).
                HasForeignKey(lm => lm.WatchListId).
                OnDelete(DeleteBehavior.Cascade);// đảm bảo ràng dữ liệu không mồ côi(khi 1 media(or watchList) bị xoá đi thì sẽ xoá tất cả các thứ liên quan đến watchlist)
            // thiết lập mối quan hệ giữa bảng ListMediaModel và MediaModel, cho phép một media thuộc về nhiều danh sách xem(watchlist).
            modelBuilder.Entity<ListMediaModel>().
                HasOne(lm => lm.media).
                WithMany(m => m.WatchLists).
                HasForeignKey(lm => lm.MediaId).
                OnDelete(DeleteBehavior.Cascade); // đảm bảo ràng dữ liệu không mồ côi(khi 1 media(or watchList) bị xoá đi thì sẽ xoá tất cả các thứ liên quan đến watchlist)

            modelBuilder.Entity<ListMediaModel>().Property(lm => lm.IsWatched).HasDefaultValue(false); // đặt thuộc tính IsWatched mặc định false
            modelBuilder.Entity<ListMediaModel>().Property(lm => lm.Favorite).HasDefaultValue(false); // đặt thuộc tính Favorite mặc định là false

            base.OnModelCreating(modelBuilder); // đảm bảo tính logic của phần tự cấu hình sẽ không bị mất (vì đây là 1 phương thức override)
        }
    }
}
