using Microsoft.AspNetCore.Mvc;
using LibraryManagement.Data;
using LibraryManagement.Models;
using LibraryManagement.ViewModels;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Controllers
{
    public class AuthorController : Controller
    {
        private readonly AppDbContext _context;

        public AuthorController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(string? search)
        {
            var authors = _context.Authors
                .Where(a => string.IsNullOrEmpty(search) || a.Name.Contains(search))
                .Select(a => new AuthorViewModel
                {
                    AuthorId = a.AuthorId,
                    Name = a.Name,
                    BookCount = _context.Books.Count(b => b.AuthorId == a.AuthorId)
                })
                .ToList();

            ViewBag.Search = search; 
            return View(authors);
        }

        public IActionResult BooksByAuthor(int authorId)
        {
            // author's book
            var books = _context.Books
                .Where(b => b.AuthorId == authorId)
                .Include(b => b.LibraryBranch) // Ensure LibraryBranch is loaded
                .Include(b => b.Customer) // Ensure Customer is loaded
                .Select(b => new BookViewModel
                {
                    BookId = b.BookId,
                    Title = b.Title,
                    LibraryBranchName = b.LibraryBranch != null ? b.LibraryBranch.BranchName : "Unknown",
                    BorrowerName = b.Customer != null
                        ? (!string.IsNullOrEmpty(b.Customer.Name) ? b.Customer.Name : b.Customer.Email)
                        : "Available",
                    CustomerId = b.CustomerId 
                })
                .ToList();;

                ViewBag.AuthorName = _context.Authors
                    .Where(a => a.AuthorId == authorId)
                    .Select(a => a.Name)
                    .FirstOrDefault();

                return View(books); // book list
            }
    }
}