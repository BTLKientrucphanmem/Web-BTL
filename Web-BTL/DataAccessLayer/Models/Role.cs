namespace Web_BTL.DataAccessLayer.Models {
    public enum Role : byte {
        SuperAdmin = 0, // admin với cấp cao nhất (với tất cả các quyền)
        Movie_Management = 1, // admin với cấp quản lý phim(push-pop phim lên web)
        CommentAndReview_Management = 2, // quản lý đánh giá và review
        ServicePackage_Management = 3// quản lý các gói dịch vụ cho user
    }
}
