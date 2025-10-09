using laptrinhweb2.Models.Domain;
using laptrinhweb2.Models.DTO;

namespace laptrinhweb2.Repositories
{
    public interface IBookRepository
    {
        List<BookDTO> GetAllBooks(string? filterOn = null, string?filterQuery = null, string? sortBy = null,bool isAscending = true, int pageNumber = 1, int pageSize = 1000);
        BookDTO GetBookById(int id);
        AddBookRequestDTO AddBook(AddBookRequestDTO addBookRequestDTO);
        AddBookRequestDTO? UpdateBookById(int id, AddBookRequestDTO bookDTO);
        Book? DeleteBookById(int id);
    }
}
