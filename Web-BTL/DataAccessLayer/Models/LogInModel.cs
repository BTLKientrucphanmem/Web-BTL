using System.ComponentModel.DataAnnotations;

namespace Web_BTL.DataAccessLayer.Models {
    public class LogInModel // dùng để khi đăng nhập và đăng kí dễ dùng ko bị gò bó bởi cái trường dữ liệu khác của User
    {
        [Required]
        public string? LogInName { get; set; }
        [Required]
        public string? Password { get; set; }
    }
}
