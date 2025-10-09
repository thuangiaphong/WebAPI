﻿using System.ComponentModel.DataAnnotations;

namespace laptrinhweb2.Models.Domain
{
    public class Book
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsRead { get; set; }
        public DateTime? DateRead { get; set; }
        public int? Rate { get; set; }
        public string Genre { get; set; }
        public string CoverUrl { get; set; }
        public DateTime DateAdded { get; set; }

        //navagation Properties - One publisher has mmany books
        public int PublisherID { get; set; }
        public Publisher Publisher { get; set; }
        //navagation Properties - One book has mmany books
        public List<Book_Author> Book_Authors { get; set; }    }
}
