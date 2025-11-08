namespace LibraryManagement.ViewModels
{
    public class CustomerDetailsViewModel
    {
        public string CustomerId { get; set; }
        public string? Name { get; set; } = string.Empty;
        
        //see the list of book that is borrowed by the customer
        public List<BookViewModel> BorrowedBooks { get; set; } = new List<BookViewModel>();
    }
}