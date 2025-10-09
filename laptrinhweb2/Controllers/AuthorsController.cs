using laptrinhweb2.Data;
using laptrinhweb2.Models.Domain;
using laptrinhweb2.Models.DTO;
using laptrinhweb2.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace laptrinhweb2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorsController : ControllerBase
    {
        private readonly AppDbContext _dbContext;
        private readonly IAuthorRepository _authorRepository;

        public AuthorsController(AppDbContext dbContext, IAuthorRepository authorRepository)
        {
            _dbContext = dbContext;
            _authorRepository = authorRepository;
        }

        [HttpGet("get-all-authors")]
        public IActionResult GetAllAuthors()
        {
            var allAuthors = _authorRepository.GellAllAuthors();
            return Ok(allAuthors);
        }

        [HttpGet("get-author-by-id/{id}")]
        public IActionResult GetAuthorById([FromRoute] int id)
        {
            var author = _authorRepository.GetAuthorById(id);
            if (author == null)
            {
                return NotFound();
            }
            return Ok(author);
        }

        [HttpPost("add-author")]
        public IActionResult AddAuthor([FromBody] AddAuthorRequestDTO addAuthorRequestDTO)
        {
            var newAuthor = _authorRepository.AddAuthor(addAuthorRequestDTO);
            return Ok(newAuthor);
        }

       
        [HttpPut("update-author-by-id/{id}")]
        public IActionResult UpdateAuthorById([FromRoute] int id, [FromBody] AuthorNoIdDTO authorDTO)
        {
            var updatedAuthor = _authorRepository.UpdateAuthorById(id, authorDTO);
            if (updatedAuthor == null)
            {
                return NotFound();
            }
            return Ok(updatedAuthor);
        }
    
        [HttpDelete("delete-author-by-id/{id}")]
        public IActionResult DeleteAuthorById([FromRoute] int id)
        {
            var deletedAuthor = _authorRepository.DeleteAuthorById(id);
            if (deletedAuthor == null)
            {
                return NotFound();
            }
            return Ok(deletedAuthor);
        }
    }
}
