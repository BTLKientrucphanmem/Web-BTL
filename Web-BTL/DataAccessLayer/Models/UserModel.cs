
using System.ComponentModel.DataAnnotations;

namespace Web_BTL.DataAccessLayer.Models {
    public abstract class UserModel {
        public string? UserName { get; set; } // tene người dùng không có cx đc
        [Required]
        public string? UserLogin { get; set; } // tên tài khoản dùng để login
        [Required]
        public string? LoginPassword { get; set; } // mật khẩu của Login
        [Required]
        public string? UserEmail { get; set; } // địa chỉ email
        public DateTime? UserCreateDate { get; set; } // ngày tạo
        public string? UserImagePath { get; set; } // đường dẫn đến ảnh của user
        public bool? UserState { get; set; } // trạng thái tài khoản idle: chờ
        public TimeSpan? UserDuration { get; set; } // thời gian hoạt động của User
    }
}
