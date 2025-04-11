
using Xabe.FFmpeg;

namespace Web_BTL.BusinessLogicLayer.Services {
    public class SaveImageVideo {
        public async Task<string> SaveImageAsync(IWebHostEnvironment _environment, string url, string oldName, string newName, IFormFile image) {
            validateEnvironment(_environment);
            validateFile(image);
            validateUrl(url);

            string uploadsFolder = Path.Combine(_environment.WebRootPath, url);

            DeleteFile(_environment, url, oldName);
            // Lấy phần mở rộng của file (ví dụ .jpg, .png)
            string fileExtension = Path.GetExtension(image.FileName);
            // Tạo tên file tùy chỉnh, ở đây đang lấy theo tên đăng nhập
            string name = newName;
            string fileName = $"{name}{fileExtension}";
            // Đường dẫn file mới
            string newFilePath = Path.Combine(uploadsFolder, fileName);
            // Lưu ảnh mới vào thư mục với tên mới
            try {
                await SaveFileAsync(image, newFilePath);
            } catch (Exception ex) {
                Console.WriteLine("Loi o phan luu anh" + ex.Message);
            }
            // Cập nhật đường dẫn ảnh mới vào cơ sở dữ liệu
            return fileName;
        }
        public async Task<(string videoName, TimeSpan? duration)> SaveVideoAsync(IWebHostEnvironment _environment, string url, string oldName, string newName, IFormFile video, bool getDuration = false) {
            validateEnvironment(_environment);
            validateFile(video);
            validateUrl(url);

            string uploadsFolder = Path.Combine(_environment.WebRootPath, url);

            DeleteFile(_environment, url, oldName);
            string fileExtension = Path.GetExtension(video.FileName);
            string name = newName;
            string fileName = $"{name}{fileExtension}";
            string newFilePath = Path.Combine(uploadsFolder, fileName);
            try {
                await SaveFileAsync(video, newFilePath);
                TimeSpan? duration = null;
                if (getDuration) {
                    var mediaInfo = await FFmpeg.GetMediaInfo(newFilePath);
                    duration = mediaInfo.Duration;
                }
                return (fileName, duration);
            } catch (Exception ex) {
                Console.WriteLine($"Khong up duoc video: {ex.Message}");
            }
            return (fileName, null); // trả về 1 tuple(struct)
        }
        public void DeleteFile(IWebHostEnvironment _environment, string url, string name) {
            validateEnvironment(_environment);
            validateUrl(url);
            if (name == "default.png") return;
            string filePath = Path.Combine(_environment.WebRootPath, url, name); // lấy đường dẫn từ wwwroot để xoá file
            if (File.Exists(filePath))
                File.Delete(filePath);
        }
        private async Task SaveFileAsync(IFormFile file, string path) {
            using (var fileStream = new FileStream(path, FileMode.Create)) {
                await file.CopyToAsync(fileStream);
            }
        }

        private void validateEnvironment(IWebHostEnvironment environment) {
            if (environment == null) throw new ArgumentNullException("Lỗi môi trường không tồn tại");
        }
        private void validateFile(IFormFile file) {
            if (file == null) throw new ArgumentNullException("Lỗi file ko tồn tại");
        }
        private void validateUrl(string url) {
            if (url == null || url == "") throw new ArgumentNullException("url không tồn tại");
        }
    }
}