using LibraryManagement.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Data
{
    public class AppDbContext : IdentityDbContext<Customer>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Author> Authors { get; set; }
        public DbSet<LibraryBranch> LibraryBranches { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Customer> Customers { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seed Authors
            modelBuilder.Entity<Author>().HasData(
                new Author { AuthorId = 1, Name = "George Orwell" },
                new Author { AuthorId = 2, Name = "J.K. Rowling" },
                new Author { AuthorId = 3, Name = "Jane Austen" }
            );

            // Seed Library Branches
            modelBuilder.Entity<LibraryBranch>().HasData(
                new LibraryBranch { LibraryBranchId = 1, BranchName = "Main Library" },
                new LibraryBranch { LibraryBranchId = 2, BranchName = "East Side Branch" }
            );

            // Seed Books
            modelBuilder.Entity<Book>().HasData(
                new Book
                {
                    BookId = 1,
                    Title = "1984",
                    AuthorId = 1,
                    LibraryBranchId = 1
                },
                new Book
                {
                    BookId = 2,
                    Title = "Harry Potter and the Philosopher's Stone",
                    AuthorId = 2,
                    LibraryBranchId = 1
                },
                new Book
                {
                    BookId = 3,
                    Title = "Pride and Prejudice",
                    AuthorId = 3,
                    LibraryBranchId = 2
                }
            );
        }
    }
}