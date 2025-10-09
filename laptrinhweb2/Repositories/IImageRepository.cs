using laptrinhweb2.Models.Domain;
namespace laptrinhweb2.Repositories
{
    public interface IImageRepository
    {
        Image Upload(Image image);
        List<Image> GetAllImages();
        (byte[], string, string) DownloadFile(int id);
    }
}
