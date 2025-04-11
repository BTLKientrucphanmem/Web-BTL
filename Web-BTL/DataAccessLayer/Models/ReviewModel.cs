using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Web_BTL.DataAccessLayer.Models {
    public class ReviewModel {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ReviewId { get; set; } // khoá chính
        public string? ReviewContent { get; set; } // nội dung
        public double? ReviewRating { get; set; } // sao đánh giá
        public DateTime? ReviewCreateDate { get; set; }

        // Định nghĩa CustomerId là khóa ngoại
        public int CustomerId { get; set; } // khoá ngoại

        // Thuộc tính điều hướng
        [ForeignKey("CustomerId")]
        public virtual CustomerModel? UserModel { get; set; }

        public int MediaId { get; set; }
        [ForeignKey("MediaId")]
        public virtual MediaModel? Medias { get; set; }
    }
}
