using laptrinhweb2.Data;
using laptrinhweb2.Models.Domain;

namespace laptrinhweb2.Repositories
{
    public class LocalImageRepository : IImageRepository
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AppDbContext _dbContext;

        public LocalImageRepository(IWebHostEnvironment webHostEnvironment, IHttpContextAccessor httpContextAccessor,
            AppDbContext dbContext)
        {
            _webHostEnvironment = webHostEnvironment;
            _httpContextAccessor = httpContextAccessor;
            _dbContext = dbContext;
        } 

        public Image Upload(Image image)
        {

            var localPath = Path.Combine(_webHostEnvironment.ContentRootPath, "Images");
            var localFilePath = Path.Combine(localPath,
                $"{image.FileName}{image.FileExtension}");

            if (!Directory.Exists(localPath))
            {

                Directory.CreateDirectory(localPath);
            }

            using (var stream = new FileStream(localFilePath, FileMode.Create))
            {
                image.File.CopyTo(stream);
            }

            // https://localhost:8080/images/image.jpg
            var urlFilePath = $"{_httpContextAccessor.HttpContext.Request.Scheme}://" +
                              $"{_httpContextAccessor.HttpContext.Request.Host}" +
                              $"{_httpContextAccessor.HttpContext.Request.PathBase}/Images/" +
                              $"{image.FileName}{image.FileExtension}";
            image.FilePath = urlFilePath;

            _dbContext.Image.Add(image);
            _dbContext.SaveChanges();

            return image;
        }

        public List<Image> GetAllImages()
        {
            return _dbContext.Image.ToList();
        }

        public (byte[], string, string) DownloadFile(int Id)
        {
            try
            {
                var FileById = _dbContext.Image.Where(x => x.Id == Id).FirstOrDefault();

                if (FileById == null)
                {

                    throw new FileNotFoundException($"Không tìm thấy bản ghi ảnh với Id: {Id} trong cơ sở dữ liệu.");
                }

                var path = Path.Combine(_webHostEnvironment.ContentRootPath, "Images",
                    $"{FileById.FileName}{FileById.FileExtension}");

                if (!File.Exists(path))
                {

                    throw new FileNotFoundException($"Không tìm thấy tệp vật lý tại đường dẫn: {path}");
                }

                var stream = File.ReadAllBytes(path);

                var fileName = FileById.FileName + FileById.FileExtension;

                return (stream, "application/octet-stream", fileName);
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }

}