using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using webMVC.Models.DTO;

namespace webMVC.Controllers
{
    public class BooksController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public BooksController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        // ------------------ HIỂN THỊ DANH SÁCH SÁCH ------------------
        public async Task<IActionResult> Index([FromQuery] string filterOn = null, string filterQuery = null, string sortBy = null, bool isAscending = true)
        {
            List<BookDTO> response = new List<BookDTO>();

            try
            {
                var client = _httpClientFactory.CreateClient("API");
                filterOn ??= string.Empty;
                filterQuery ??= string.Empty;
                sortBy ??= string.Empty;

                var httpResponse = await client.GetAsync(
                    $"Books/get-all-books?filterOn={filterOn}&filterQuery={filterQuery}&sortBy={sortBy}&isAscending={isAscending}"
                );

                httpResponse.EnsureSuccessStatusCode();
                var items = await httpResponse.Content.ReadFromJsonAsync<IEnumerable<BookDTO>>();
                if (items != null)
                    response.AddRange(items);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View("Error");
            }

            return View(response);
        }

        // ------------------ THÊM SÁCH ------------------
        [HttpPost]
        public async Task<IActionResult> addBook(AddBookDTO addBookDTO)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("API");
                addBookDTO.DateAdded = DateTime.Now;

                var httpRequest = new HttpRequestMessage(HttpMethod.Post, "Books/add-book")
                {
                    Content = new StringContent(JsonSerializer.Serialize(addBookDTO), Encoding.UTF8, MediaTypeNames.Application.Json)
                };

                var httpResponse = await client.SendAsync(httpRequest);
                httpResponse.EnsureSuccessStatusCode();

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View("Error");
            }
        }
        // ------------------ HIỂN THỊ FORM THÊM SÁCH ------------------
        [HttpGet]
        public async Task<IActionResult> Add()
        {
            try
            {
                var client = _httpClientFactory.CreateClient("API");

                var authors = await client.GetFromJsonAsync<IEnumerable<authorDTO>>("Authors/get-all-authors");
                var publishers = await client.GetFromJsonAsync<IEnumerable<publisherDTO>>("Publishers/get-all-publishers");

                ViewBag.ListAuthor = authors;
                ViewBag.ListPublisher = publishers;

                return View("addBook", new AddBookDTO());
            }
            catch (HttpRequestException httpEx)
            {
                ViewBag.Error = $"HTTP Request error: {httpEx.Message}";
                return View("Error");
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View("Error");
            }
        }

        // ------------------ CHI TIẾT SÁCH ------------------
        public async Task<IActionResult> listBook(int id)
        {
            BookDTO response = new BookDTO();

            try
            {
                var client = _httpClientFactory.CreateClient("API");
                var httpResponse = await client.GetAsync($"Books/get-book-by-id/{id}");
                httpResponse.EnsureSuccessStatusCode();

                var item = await httpResponse.Content.ReadFromJsonAsync<BookDTO>();
                if (item != null)
                    response = item;
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View("Error");
            }

            return View(response);
        }

        // ------------------ SỬA SÁCH (EDIT) ------------------
        [HttpGet]
        public async Task<IActionResult> editBook(int id)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("API");

                // Lấy thông tin sách theo ID
                var httpResponseBook = await client.GetAsync($"Books/get-book-by-id/{id}");
                httpResponseBook.EnsureSuccessStatusCode();
                var book = await httpResponseBook.Content.ReadFromJsonAsync<BookDTO>();
                ViewBag.Book = book;

                // Lấy danh sách tác giả
                var httpResponseAuthors = await client.GetAsync("Authors/get-all-authors");
                httpResponseAuthors.EnsureSuccessStatusCode();
                var authors = await httpResponseAuthors.Content.ReadFromJsonAsync<IEnumerable<authorDTO>>();
                ViewBag.ListAuthor = authors;

                // Lấy danh sách nhà xuất bản
                var httpResponsePublishers = await client.GetAsync("Publishers/get-all-publishers");
                httpResponsePublishers.EnsureSuccessStatusCode();
                var publishers = await httpResponsePublishers.Content.ReadFromJsonAsync<IEnumerable<publisherDTO>>();
                ViewBag.ListPublisher = publishers;

                return View();
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View("Error");
            }
        }
        [HttpPost]
        public async Task<IActionResult> editBook([FromRoute] int id, editDTO bookDTO)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("API");

                var json = JsonSerializer.Serialize(bookDTO);
                Console.WriteLine("JSON gửi đi khi sửa sách: " + json);

                // Gửi yêu cầu PUT để cập nhật sách theo id
                var httpRequest = new HttpRequestMessage(HttpMethod.Put, $"Books/update-book-by-id/{id}")
                {
                    Content = new StringContent(JsonSerializer.Serialize(bookDTO), Encoding.UTF8, MediaTypeNames.Application.Json)
                };

                var httpResponse = await client.SendAsync(httpRequest);
                httpResponse.EnsureSuccessStatusCode();

                // Nếu cập nhật thành công → quay lại danh sách
                return RedirectToAction("Index", "Books");
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View("Error");
            }
        }
        [HttpGet]
        public async Task<IActionResult> delBook([FromRoute] int id)
        {
            try
            {
                // lấy dữ liệu books from API
                var client = _httpClientFactory.CreateClient("API");
                var httpResponseMess = await client.DeleteAsync($"Books/delete-book-by-id/{id}");
                httpResponseMess.EnsureSuccessStatusCode();
                return RedirectToAction("Index", "Books");
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
            }
            return View("Index");
        }

    }
}
