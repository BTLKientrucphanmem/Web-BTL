using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Web_BTL.DataAccessLayer.Models {
    public class CustomerModel : UserModel {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CustomerId { get; set; }
        public virtual ICollection<ReviewModel> Reviews { get; set; } // liên kết đến bảng review
        public virtual int? HistoryListId { get; set; } // khoá phụ bảng History List
        public virtual int? FavoriteListId { get; set; } // khoá phụ bảng Favorite List
        public virtual int? WatchListId { get; set; } // khoá phụ bảng Watch List
        public virtual WatchListModel? WatchList { get; set; }
    }
}
