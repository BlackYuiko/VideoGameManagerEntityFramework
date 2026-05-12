using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using VideoGameManager.Data;
using VideoGameManager.Models;

namespace VideoGameManager.Pages.Developers
{
    public class DeleteModel : PageModel
    {
        private readonly GameStoreContext _context;

        [BindProperty]
        public Developer Developer { get; set; }

        public DeleteModel(GameStoreContext context) => _context = context;

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Developer = await _context.Developers
                .Include(d => d.Games)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (Developer == null) return NotFound();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var developerToDelete = await _context.Developers
                .Include(d => d.Games)
                .FirstOrDefaultAsync(m => m.Id == Developer.Id);

            if (developerToDelete.Games != null && developerToDelete.Games.Any())
            {
                ModelState.AddModelError(string.Empty, "Cannot delete a developer that has associated games.");
                Developer = developerToDelete;
                return Page();
            }

            _context.Developers.Remove(developerToDelete);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}