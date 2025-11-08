
//secondary window that shows the list of books borrowed by the customer, and it provides checkout and return functions
namespace LibraryManagement.ViewModels
{
    public class CustomerViewModel
    {
        public string CustomerId { get; set; }
        public string Name { get; set; } 
        public string Email { get; set; } 
        public string Phone { get; set; } 
        public int BorrowedBooks { get; set; } 
        public string? ReturnUrl { get; set; } //for returning to the original touchpoint
    }
}