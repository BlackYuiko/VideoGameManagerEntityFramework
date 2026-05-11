using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using VideoGameManager.Models;
using VideoGameManager.Services;

namespace VideoGameManager.Pages.Games
{
    public class DeleteModel : PageModel
    {
        private readonly GameService _gameService;

        public Game? Game { get; set; }

        public DeleteModel(GameService gameService)
        {
            _gameService = gameService;
        }

        // l'usuari fa clic a "Esborrar" des de la llista
        public IActionResult OnGet(int id)
        {
            Game = _gameService.GetById(id);

            if (Game == null)
            {
                return NotFound();
            }

            return Page();
        }

        // l'usuari fa clic al botó vermell de confirmació
        public IActionResult OnPost(int id)
        {
            var gameToDelete = _gameService.GetById(id);

            if (gameToDelete != null)
            {
                _gameService.Delete(id);
            }

            return RedirectToPage("./Index");
        }
    }
}