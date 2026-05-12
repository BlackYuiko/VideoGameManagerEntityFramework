using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using VideoGameManager.Data;
using VideoGameManager.Models;

namespace VideoGameManager.Pages.Developers
{
    public class DetailsModel : PageModel
    {
        private readonly GameStoreContext _context;
        public Developer Developer { get; set; }

        public DetailsModel(GameStoreContext context) => _context = context;

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Developer = await _context.Developers.Include(d => d.Games).FirstOrDefaultAsync(x => x.Id == id);

            if (Developer == null) return NotFound();

            return Page();
        }
    }
}