namespace webMVC.Models.DTO
{
    public class BookDTO
    {
        public int Id { get; set; } // [cite: 342]
        public string? Title { get; set; } // [cite: 344]
        public string? Description { get; set; } // [cite: 346]
        public bool IsRead { get; set; } // [cite: 348]
        public DateTime? DateRead { get; set; } // [cite: 350]
        public int? Rate { get; set; } // [cite: 352]
        public string? Genre { get; set; } // [cite: 354]
        public string? CoverUrl { get; set; } // [cite: 356]
        public DateTime DateAdded { get; set; } // [cite: 357]
        public string PublisherName { get; set; } // [cite: 359]
        public List<string> AuthorNames { get; set; } // [cite: 360]
    }
}
