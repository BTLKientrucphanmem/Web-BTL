using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Web_BTL.DataAccessLayer.Models {
    public class GenreModel {
        public GenreModel() {
            Medias = new HashSet<MediaModel>();
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int GenreId { get; set; }
        public string? Type { get; set; }
        public virtual ICollection<MediaModel> Medias { get; set; }
    }
}
