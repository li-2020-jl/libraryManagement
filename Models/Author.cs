namespace LibraryManagement.Models
{
    public class Author
    {
        public int AuthorId { get; set; }
        public required string Name { get; set; }

        //author's book collection
        public List<Book>? Books { get; set; }
    }
}