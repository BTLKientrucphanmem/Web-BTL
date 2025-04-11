namespace Web_BTL.DataAccessLayer.Models {
    public class ListMediaModel {
        // nối đến bảng watchList
        public int? WatchListId { get; set; }
        public WatchListModel? watchList { get; set; }
        // nối đến bảng media
        public int? MediaId { get; set; }
        public MediaModel? media { get; set; }
        // các thông tin chi tiết
        public bool? IsWatched { get; set; } = false;
        public bool? Favorite { get; set; } = false;
        public DateTime? AddDate { get; set; }
    }
}