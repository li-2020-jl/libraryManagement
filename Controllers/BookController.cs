using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LibraryManagement.Data;
using LibraryManagement.Models;
using LibraryManagement.ViewModels;
using System.Linq;

namespace LibraryManagement.Controllers
{
    public class BookController : Controller
    {
        private readonly AppDbContext _context;

        public BookController(AppDbContext context)
        {
            _context = context;
        }

    public async Task<IActionResult> Index(string? search)
    {
        var query = _context.Books
            .Include(b => b.Author)
            .Include(b => b.LibraryBranch)
            .Include(b => b.Customer)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(b => b.Title.Contains(search));
        }

        var books = await query
            .Select(b => new BookViewModel
            {
                BookId = b.BookId,
                Title = b.Title,
                CoverImageUrl = b.CoverImageUrl,
                AuthorId = b.AuthorId,
                AuthorName = b.Author != null ? b.Author.Name : "Unknown",
                LibraryBranchId = b.LibraryBranchId,
                LibraryBranchName = b.LibraryBranch != null ? b.LibraryBranch.BranchName : "Unknown",
                BorrowerName = b.Customer != null
                    ? (!string.IsNullOrEmpty(b.Customer.Name) ? b.Customer.Name : b.Customer.Email)
                    : "Available",
                CustomerId = b.CustomerId,
            })
            .ToListAsync();

        ViewBag.Search = search; 
        return View(books);
    }

        public IActionResult Create()
        {
            ViewBag.Authors = _context.Authors.ToList();
            ViewBag.Branches = _context.LibraryBranches.ToList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(BookViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Authors = _context.Authors.ToList();
                ViewBag.Branches = _context.LibraryBranches.ToList();
                return View(model);
            }

            var newBook = new Book
            {
                Title = model.Title,
                AuthorId = model.AuthorId,
                LibraryBranchId = model.LibraryBranchId
            };

            _context.Books.Add(newBook);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Checkout(int bookId)
        {
            var customerId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(customerId))
                return Unauthorized(); // not loged in!

            var book = _context.Books.Find(bookId);

            if (book == null)
                return NotFound();

            if (!string.IsNullOrEmpty(book.CustomerId))
                return BadRequest("This book is already checked out.");

            book.CustomerId = customerId;
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ReturnBook(int bookId)
        {
            var customerId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            var book = _context.Books.Find(bookId);

            if (book == null)
                return NotFound();

            if (book.CustomerId != customerId)
                return Unauthorized(); // 只能自己还自己借的书

            book.CustomerId = null;
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}