using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Web_BTL.DataAccessLayer.Models {
    public class ActorModel {
        public ActorModel() {
            Medias = new HashSet<Actor_MediaModel>();
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ActorID { get; set; } // khoá
        public string? ActorName { get; set; } // tên actor
        public DateTime? AcctorDate { get; set; } // ngày sinh
        public virtual ICollection<Actor_MediaModel> Medias { get; set; } // kết nối đến bảng phụ
    }
}