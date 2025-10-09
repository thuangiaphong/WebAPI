using laptrinhweb2.Data;
using laptrinhweb2.Models.Domain;
using laptrinhweb2.Models.DTO;

namespace laptrinhweb2.Repositories
{
    public class SQLPublisherRepository : IPublisherRepository
    {
        private readonly AppDbContext _dbContext;

        public SQLPublisherRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public AddPublisherRequestDTO AddPublisher(AddPublisherRequestDTO addPublisherDTO)
        {
            var publisher = new Publisher
            {
                Name = addPublisherDTO.Name
            };

            _dbContext.Publishers.Add(publisher);
            _dbContext.SaveChanges();

            return addPublisherDTO;
        }

        public Publisher? DeletePublisherById(int id)
        {
            var publisher = _dbContext.Publishers.Find(id);
            if (publisher != null)
            {
                _dbContext.Publishers.Remove(publisher);
                _dbContext.SaveChanges();
                return publisher;
            }
            return null;
        }

        public List<PublisherDTO> GetAllPublishers()
        {
            var publishers = _dbContext.Publishers
                .Select(p => new PublisherDTO
                {
                    Id = p.Id,
                    Name = p.Name
                }).ToList();

            return publishers;
        }

        public PublisherNoIdDTO? GetPublisherById(int id)
        {
            var publisher = _dbContext.Publishers.Find(id);
            if (publisher != null)
            {
                return new PublisherNoIdDTO
                {
                    Name = publisher.Name
                };
            }
            return null;
        }

        public PublisherNoIdDTO? UpdatePublisherById(int id, PublisherNoIdDTO publisherDTO)
        {
            var publisher = _dbContext.Publishers.Find(id);
            if (publisher != null)
            {
                publisher.Name = publisherDTO.Name;
                _dbContext.SaveChanges();

                return publisherDTO;
            }
            return null;
        }
    }
}