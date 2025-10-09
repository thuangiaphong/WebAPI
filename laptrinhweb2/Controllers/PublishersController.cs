using laptrinhweb2.Data;
using laptrinhweb2.Models.Domain;
using laptrinhweb2.Models.DTO;
using laptrinhweb2.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace laptrinhweb2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PublishersController : ControllerBase
    {
        private readonly AppDbContext _dbContext;
        private readonly IPublisherRepository _publisherRepository;

        public PublishersController(AppDbContext dbContext, IPublisherRepository publisherRepository)
        {
            _dbContext = dbContext;
            _publisherRepository = publisherRepository;
        }

        // GET ALL
        [HttpGet("get-all-publishers")]
        public IActionResult GetAllPublisher()
        {
            var publishers = _publisherRepository.GetAllPublishers();
            return Ok(publishers);
        }

        // GET BY ID
        [HttpGet("get-publisher-by-id/{id}")]
        public IActionResult GetPublisherById([FromRoute] int id)
        {
            var publisher = _publisherRepository.GetPublisherById(id);
            if (publisher == null)
            {
                return NotFound();
            }
            return Ok(publisher);
        }

        // ADD
        [HttpPost("add-publisher")]
        public IActionResult AddPublisher([FromBody] AddPublisherRequestDTO addPublisherDTO)
        {
            var newPublisher = _publisherRepository.AddPublisher(addPublisherDTO);
            return Ok(newPublisher);
        }

        // UPDATE
        [HttpPut("update-publisher-by-id/{id}")]
        public IActionResult UpdatePublisherById([FromRoute] int id, [FromBody] PublisherNoIdDTO publisherDTO)
        {
            var updatedPublisher = _publisherRepository.UpdatePublisherById(id, publisherDTO);
            if (updatedPublisher == null)
            {
                return NotFound();
            }
            return Ok(updatedPublisher);
        }

        // DELETE
        [HttpDelete("delete-publisher-by-id/{id}")]
        public IActionResult DeletePublisherById([FromRoute] int id)
        {
            var deletedPublisher = _publisherRepository.DeletePublisherById(id);
            if (deletedPublisher == null)
            {
                return NotFound();
            }
            return Ok(deletedPublisher);
        }
    }
}
