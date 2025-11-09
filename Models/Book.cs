using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryManagement.Models
{
    public class Book
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BookId { get; set; }

        [Required]
        public string? Title { get; set; }

        // Book cover image URL
        public string? CoverImageUrl { get; set; }

        // show who the author is
        [Required]
        public int AuthorId { get; set; }
        
        [ForeignKey("AuthorId")]
        public Author? Author { get; set; }

        // show which branch is the book in
        [Required]
        public int LibraryBranchId { get; set; }

        [ForeignKey("LibraryBranchId")]
        public LibraryBranch? LibraryBranch { get; set; }

        // show who borrowed this book, later for the lending function in the book list
        public string? CustomerId { get; set; } 

        [ForeignKey("CustomerId")]
        public Customer? Customer { get; set; } 
    }
}