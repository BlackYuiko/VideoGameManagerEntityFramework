using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using VideoGameManager.Data;
using VideoGameManager.Models;


namespace VideoGameManager.Pages.Games
{
    public class DetailsModel : PageModel
    {
        private readonly GameStoreContext _context;

        public Game? Game { get; set; }

        public DetailsModel(GameStoreContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> OnGet(int id)
        {
            Game = await _context.Games.Include(g => g.Developer).FirstOrDefaultAsync(x => x.Id == id);

            if (Game == null)
            {
                return NotFound();
            }

            return Page();
        }
    }
}