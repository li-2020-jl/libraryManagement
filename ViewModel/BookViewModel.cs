namespace LibraryManagement.ViewModels
{
    public class BookViewModel
    {
        public int BookId { get; set; }
        public string? Title { get; set; }

        public int AuthorId { get; set; } 
        public string? AuthorName { get; set; }

        public int LibraryBranchId { get; set; }
        public string? LibraryBranchName { get; set; } = "Unknown"; 

        public string? BorrowerName { get; set; }  //display the person who borrowed the book
        public string? CustomerId { get; set; } //this is for borrowing books
    }
}