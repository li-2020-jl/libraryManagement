using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LibraryManagement.Data;
using LibraryManagement.ViewModels;
using LibraryManagement.Models;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace LibraryManagement.Controllers
{
    [Authorize]
    public class CustomerController : Controller
    {
        private readonly AppDbContext _context;
        private readonly SignInManager<Customer> _signInManager;

        public CustomerController(AppDbContext context, SignInManager<Customer> signInManager)
        {
            _context = context;
            _signInManager = signInManager;
        }

        public IActionResult Details(string id)
        {
            var customer = _context.Customers
                .Include(c => c.BorrowedBooks)
                .ThenInclude(b => b.Author)
                .Include(c => c.BorrowedBooks)
                .ThenInclude(b => b.LibraryBranch)
                .FirstOrDefault(c => c.Id == id);

            if (customer == null) return NotFound();

            var viewModel = new CustomerDetailsViewModel
            {
                CustomerId = customer.Id,
                Name = customer.Name,
                BorrowedBooks = customer.BorrowedBooks?.Select(b => new BookViewModel
                {
                    BookId = b.BookId,
                    Title = b.Title,
                    AuthorName = b.Author?.Name ?? "Unknown",
                    LibraryBranchName = b.LibraryBranch?.BranchName ?? "Unknown",
                    CustomerId = b.CustomerId
                }).ToList() ?? new List<BookViewModel>()
            };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ReturnBook(int bookId)
        {
            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var book = _context.Books.FirstOrDefault(b => b.BookId == bookId && b.CustomerId == currentUserId);

            if (book == null)
                return Unauthorized("You cannot return a book you didn't borrow.");

            book.CustomerId = null;
            _context.SaveChanges();

            return RedirectToAction("Details", new { id = currentUserId });
        }

        public IActionResult Index(string searchTerm)
        {
            var customers = _context.Customers
                .Where(c => string.IsNullOrEmpty(searchTerm) || c.Name.Contains(searchTerm) || c.Email.Contains(searchTerm))
                .Select(c => new CustomerViewModel
                {
                    CustomerId = c.Id,
                    Name = c.Name,
                    Email = c.Email,
                    Phone = c.PhoneNumber,
                    BorrowedBooks = c.BorrowedBooks.Count()
                })
                .ToList();

            return View(customers);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Checkout(int bookId)
        {
            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var book = _context.Books.FirstOrDefault(b => b.BookId == bookId && b.CustomerId == null);

            if (book == null)
                return BadRequest("Book is already checked out or does not exist.");

            book.CustomerId = currentUserId;
            _context.SaveChanges();

            return RedirectToAction("Details", new { id = currentUserId });
        }

        public IActionResult Edit(string id, string? returnUrl = null)
        {
            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (id != currentUserId)
                return Unauthorized();

            var customer = _context.Customers.Find(id);
            if (customer == null) return NotFound();

            var viewModel = new CustomerViewModel
            {
                CustomerId = customer.Id,
                Name = customer.Name,
                Email = customer.Email,
                Phone = customer.PhoneNumber,
                ReturnUrl = returnUrl
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, CustomerViewModel model)
        {
            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (id != currentUserId)
                return Unauthorized();

            if (!ModelState.IsValid) return View(model);

            var customer = _context.Customers.Find(id);
            if (customer == null) return NotFound();

            customer.Name = model.Name;
            customer.Email = model.Email;
            customer.PhoneNumber = model.Phone;
            customer.UserName = model.Name;

            _context.Update(customer);
            _context.SaveChanges();

            await _signInManager.RefreshSignInAsync(customer);

            if (!string.IsNullOrEmpty(model.ReturnUrl)) //return to the original touchpoint
                return Redirect(model.ReturnUrl);
            else
                return RedirectToAction("Details", new { id = currentUserId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(string customerId)
        {
            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (customerId != currentUserId)
                return Unauthorized();

            var customer = _context.Customers.Find(customerId);
            if (customer == null) return NotFound();

            _context.Customers.Remove(customer);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}