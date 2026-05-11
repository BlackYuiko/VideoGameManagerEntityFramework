using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VideoGameManager.Data;
using VideoGameManager.Models;

namespace VideoGameManager.Pages.Games
{
    public class CreateModel : PageModel
    {
        public SelectList DeveloperList { get; set; }

        private readonly GameStoreContext _context;

        // [BindProperty] indica que la propietat Game s'ha de poblar
        // automàticament amb les dades del formulari quan arriba el POST.
        [BindProperty]
        public Game Game { get; set; } = new Game();

        public CreateModel(GameStoreContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var developersList = await _context.Developers.ToListAsync();
            DeveloperList = new SelectList(developersList, "Id", "Name");
            return Page();
        }

        // Rebre les dades quan l'usuari fa clic a "Guardar"
        public async Task<IActionResult> OnPost()
        {
            if(!ModelState.IsValid)
{
                var developers = await _context.Developers.ToListAsync();
                DeveloperList = new SelectList(developers, "Id", "Name");
                return Page();
            }

            _context.Games.Add(Game);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}