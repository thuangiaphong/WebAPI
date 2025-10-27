﻿using System.ComponentModel.DataAnnotations;

namespace webMVC.Models.DTO
{
    public class AddBookDTO
    {
        [Required, MinLength(10)]
        public string? Title { get; set; }
        public string? Description { get; set; }
        public bool IsRead { get; set; }
        public DateTime? DateRead { get; set; }

        [Range(0, 5, ErrorMessage = "From 0 to 5")]
        public int? Rate { get; set; }

        public string? Genre { get; set; }
        public string? CoverUrl { get; set; }

        public DateTime DateAdded { get; set; } = DateTime.Now;

        public int PublisherID { get; set; }

        public List<int> AuthorIds { get; set; } = new List<int>();
    }
}
