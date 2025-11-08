using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryManagement.Models
{
    public class Customer : IdentityUser
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty; 

        [NotMapped]
        public string CustomerId => Id;

        //borrowed which book
        public ICollection<Book> BorrowedBooks { get; set; } = new List<Book>();
    }
}