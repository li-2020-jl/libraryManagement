using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LibraryManagement.Data;
using LibraryManagement.Models;

namespace LibraryManagement.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class LibraryBranchApiController : ControllerBase
    {
        private readonly AppDbContext _context;

        public LibraryBranchApiController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/LibraryBranchApi
        [HttpGet]
        public async Task<IActionResult> GetAllBranches()
        {
            var branches = await _context.LibraryBranches.ToListAsync();
            return Ok(branches);
        }

        // GET: api/LibraryBranchApi/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBranch(int id)
        {
            var branch = await _context.LibraryBranches.FindAsync(id);
            if (branch == null)
                return NotFound();
            return Ok(branch);
        }

        // POST: api/LibraryBranchApi
        [HttpPost]
        public async Task<IActionResult> CreateBranch([FromBody] LibraryBranch branch)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _context.LibraryBranches.Add(branch);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetBranch), new { id = branch.LibraryBranchId }, branch);
        }

        // PUT: api/LibraryBranchApi/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBranch(int id, [FromBody] LibraryBranch branch)
        {
            if (id != branch.LibraryBranchId)
                return BadRequest("Branch ID mismatch");

            _context.Entry(branch).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.LibraryBranches.Any(b => b.LibraryBranchId == id))
                    return NotFound();
                throw;
            }

            return NoContent();
        }

        // DELETE: api/LibraryBranchApi/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBranch(int id)
        {
            var branch = await _context.LibraryBranches.FindAsync(id);
            if (branch == null)
                return NotFound();

            _context.LibraryBranches.Remove(branch);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}