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

        [BindProperty]
        public Game Game { get; set; } = new Game();

        [BindProperty]
        public string? NewDeveloperName { get; set; }

        [BindProperty]
        public string? NewDeveloperCountry { get; set; }

        [BindProperty]
        public int? NewDeveloperFoundedYear { get; set; }

        public CreateModel(GameStoreContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            await LoadDevelopersAsync();
            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            if (!string.IsNullOrWhiteSpace(NewDeveloperName))
            {
                ModelState.Remove("Game.DeveloperId");
            }

            if (!ModelState.IsValid)
            {
                await LoadDevelopersAsync();
                return Page();
            }

            if (!string.IsNullOrWhiteSpace(NewDeveloperName))
            {
                var newDev = new Developer 
                { 
                    Name = NewDeveloperName,
                    Country = NewDeveloperCountry,
                    FoundedYear = NewDeveloperFoundedYear ?? 0
                };
                _context.Developers.Add(newDev);
                await _context.SaveChangesAsync();

                Game.DeveloperId = newDev.Id;
            }

            _context.Games.Add(Game);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }

        private async Task LoadDevelopersAsync()
        {
            var developers = await _context.Developers.ToListAsync();
            DeveloperList = new SelectList(developers, "Id", "Name");

        }
    }
}