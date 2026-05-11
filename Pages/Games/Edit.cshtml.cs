using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using VideoGameManager.Models;
using VideoGameManager.Services;

namespace VideoGameManager.Pages.Games
{
    public class EditModel : PageModel
    {
        private readonly GameService _gameService;

        [BindProperty]
        public Game? Game { get; set; }

        public EditModel(GameService gameService)
        {
            _gameService = gameService;
        }

        // Carregar les dades del joc a editar
        public IActionResult OnGet(int id)
        {
            Game = _gameService.GetById(id);

            if (Game == null)
            {
                return NotFound();
            }

            return Page();
        }

        // Rebre les dades modificades
        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Actualitzem usant el servei (recorda que Game ara conté les dades del formulari)
            _gameService.Update(Game!);
            return RedirectToPage("./Index");
        }
    }
}