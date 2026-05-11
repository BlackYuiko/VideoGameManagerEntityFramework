using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using VideoGameManager.Models;
using VideoGameManager.Services;

namespace VideoGameManager.Pages.Games
{
    public class DetailsModel : PageModel
    {
        private readonly GameService _gameService;

        public Game? Game { get; set; }

        public DetailsModel(GameService gameService)
        {
            _gameService = gameService;
        }

        public IActionResult OnGet(int id)
        {
            Game = _gameService.GetById(id);

            if (Game == null)
            {
                return NotFound();
            }

            return Page();
        }
    }
}