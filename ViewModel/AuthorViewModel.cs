namespace LibraryManagement.ViewModels
{
    public class AuthorViewModel
    {
        public int AuthorId { get; set; }
        public string? Name { get; set; }
        public int BookCount { get; set; }  //num of book by the author

        // generate an URL to direct to the list of author's book
        public string BookListUrl => $"/Author/BooksByAuthor?authorId={AuthorId}";
    }
}