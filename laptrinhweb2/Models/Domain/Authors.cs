﻿using System.ComponentModel.DataAnnotations;

namespace laptrinhweb2.Models.Domain
{
    public class Author
    {
        [Key]
        public int Id { get; set; }
        public string FullName { get; set; }
        public List<Book_Author> Book_Authors { get; set; }
    }
}
