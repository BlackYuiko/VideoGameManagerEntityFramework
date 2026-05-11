using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VideoGameManager.Data;
using VideoGameManager.Models;

namespace VideoGameManager.Pages.Games
{
    public class EditModel : PageModel
    {
        public SelectList DeveloperList { get; set; }


        private readonly GameStoreContext _context;

        [BindProperty]
        public Game? Game { get; set; }

        public EditModel(GameStoreContext context)
        {
            _context = context;
        }

        // Carregar les dades del joc a editar
        public async Task<IActionResult> OnGetAsync(int id)
        {
            // 2. Le pasamos el 'id' al método FindAsync
            Game = await _context.Games.FindAsync(id);

            if (Game == null)
            {
                return NotFound();
            }

            var developers = await _context.Developers.ToListAsync();
            DeveloperList = new SelectList(developers, "Id", "Name");

            return Page();
        }

        // Rebre les dades modificades
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                var developers = await _context.Developers.ToListAsync();
                DeveloperList = new SelectList(developers, "Id", "Name");
                return Page();
            }

            _context.Attach(Game).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");

        }
    }
}