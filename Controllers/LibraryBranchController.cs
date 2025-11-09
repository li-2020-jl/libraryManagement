using Microsoft.AspNetCore.Mvc;
using LibraryManagement.Data;
using LibraryManagement.ViewModels;
using LibraryManagement.Models;
using System.Linq;

namespace LibraryManagement.Controllers
{
    public class LibraryBranchController : Controller
    {
        private readonly AppDbContext _context;

        public LibraryBranchController(AppDbContext context)
        {
            _context = context;
        }
        
        public IActionResult Index(string? search)
        {
            var query = _context.LibraryBranches.AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(b => b.BranchName.Contains(search));
            }

            var branches = query
                .Select(branch => new LibraryBranchViewModel
                {
                    LibraryBranchId = branch.LibraryBranchId,
                    BranchName = branch.BranchName,
                    TotalBooks = _context.Books.Count(b => b.LibraryBranchId == branch.LibraryBranchId) 
                })
                .ToList();

            ViewBag.Search = search;
            return View(branches);
        }
        
        // 添加新分馆
        public IActionResult Create()
        {
            return View(); 
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(LibraryBranchViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var newBranch = new LibraryBranch
            {
                BranchName = model.BranchName
            };

            _context.LibraryBranches.Add(newBranch);
            _context.SaveChanges();

            return RedirectToAction("Index"); // Redirect back to list after saving
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, LibraryBranchViewModel model)
        {
            if (ModelState.IsValid)
            {
                var branch = _context.LibraryBranches.Find(id);
                if (branch == null) return NotFound();
                
                branch.BranchName = model.BranchName;

                _context.Update(branch);
                _context.SaveChanges();

                return RedirectToAction("Index");
            }
            return View(model);
        }

        // 修改分馆信息
        public IActionResult Edit(int id)
        {
            var branch = _context.LibraryBranches.Find(id);
            if (branch == null) return NotFound();

            // 转换成 ViewModel
            var viewModel = new LibraryBranchViewModel
            {
                LibraryBranchId = branch.LibraryBranchId,
                BranchName = branch.BranchName
            };

            return View(viewModel); //edit view
        }

        // GET: LibraryBranch/Delete/{id}
        public IActionResult Delete(int id)
        {
            var branch = _context.LibraryBranches.Find(id);
            if (branch == null) return NotFound();

            var viewModel = new LibraryBranchViewModel
            {
                LibraryBranchId = branch.LibraryBranchId,
                BranchName = branch.BranchName
            };

            return View(viewModel); //delete view, user needs to confirm
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("LibraryBranch/DeleteConfirmed/{id}")]
        public IActionResult DeleteConfirmed(int id)
        {
            var branch = _context.LibraryBranches.Find(id);
            if (branch == null) return NotFound();

            _context.LibraryBranches.Remove(branch);
            _context.SaveChanges();

            return RedirectToAction("Index", "LibraryBranch");  // redirects to main branches page
        }
    }
}