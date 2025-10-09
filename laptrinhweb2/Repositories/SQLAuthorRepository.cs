using laptrinhweb2.Data;
using laptrinhweb2.Models.Domain;
using laptrinhweb2.Models.DTO;

namespace laptrinhweb2.Repositories
{
    public class SQLAuthorRepository : IAuthorRepository
    {
        private readonly AppDbContext _dbContext;
        public SQLAuthorRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public AddAuthorRequestDTO AddAuthor(AddAuthorRequestDTO addAuthorRequestDTO)
        {
            var author = new Author()
            {
                FullName = addAuthorRequestDTO.FullName
            };

            _dbContext.Authors.Add(author);
            _dbContext.SaveChanges();

            return addAuthorRequestDTO;
        }

        public Author? DeleteAuthorById(int id)
        {
            var author = _dbContext.Authors.Find(id);
            if (author != null)
            {
                _dbContext.Authors.Remove(author);
                _dbContext.SaveChanges();
                return author;
            }
            return null;
        }

        public List<AuthorDTO> GellAllAuthors()
        {
            var authors = _dbContext.Authors.Select(a => new AuthorDTO()
            {
                Id = a.Id,
                FullName = a.FullName
            }).ToList();

            return authors;
        }

        public AuthorNoIdDTO GetAuthorById(int id)
        {
            var author = _dbContext.Authors.Find(id);
            if (author != null)
            {
                return new AuthorNoIdDTO()
                {
                    FullName = author.FullName
                };
            }

            return new AuthorNoIdDTO() { FullName = string.Empty };
        }

        public AuthorNoIdDTO UpdateAuthorById(int id, AuthorNoIdDTO authorNoIdDTO)
        {
            var author = _dbContext.Authors.Find(id);
            if (author != null)
            {
                author.FullName = authorNoIdDTO.FullName;
                _dbContext.SaveChanges();

                return authorNoIdDTO;
            }
            return new AuthorNoIdDTO() { FullName = string.Empty };
        }
    }
}
