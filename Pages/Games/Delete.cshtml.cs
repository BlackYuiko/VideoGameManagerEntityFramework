using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using VideoGameManager.Data;
using VideoGameManager.Models;


namespace VideoGameManager.Pages.Games
{
    public class DeleteModel : PageModel
    {
        private readonly GameStoreContext _context;

        public Game? Game { get; set; }

        public DeleteModel(GameStoreContext context)
        {
            _context = context;
        }

        // l'usuari fa clic a "Esborrar" des de la llista
        public async Task<IActionResult> OnGet(int id)
        {
            Game = await _context.Games.Include(g => g.Developer).FirstOrDefaultAsync(x => x.Id == id);

            if (Game == null)
            {
                return NotFound();
            }

            return Page();
        }

        // l'usuari fa clic al botó vermell de confirmació
        public async Task<IActionResult> OnPostAsync(int id)
        {
            var gameToDelete = await _context.Games.FindAsync(id);

            if (gameToDelete != null)
            {
                try
                {
                    _context.Games.Remove(gameToDelete);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateException)
                {
                    return RedirectToPage("./Delete", new { id = id, saveChangesError = true });
                }
            }

            return RedirectToPage("./Index");
        }
    }
}