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
        } // constructor
        public Image Upload(Image image)
        {
            var localFilePath = Path.Combine(_webHostEnvironment.ContentRootPath, "Images",
                $"{image.FileName}{image.FileExtension}");

            // upload Image to local Path
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

            //add Image to the Images table
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

                var path = Path.Combine(_webHostEnvironment.ContentRootPath, "Images",
                    $"{FileById.FileName}{FileById.FileExtension}");

                var stream = File.ReadAllBytes(path);

                var fileName = FileById.FileName + FileById.FileExtension;

                return (stream, "application/octet-stream", fileName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }

}
