using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using VideoGameManager.Models;
using VideoGameManager.Services;

namespace VideoGameManager.Pages.Games
{
    public class CreateModel : PageModel
    {
        private readonly GameService _gameService;

        // [BindProperty] indica que la propietat Game s'ha de poblar
        // automàticament amb les dades del formulari quan arriba el POST.
        [BindProperty]
        public Game Game { get; set; } = new Game();

        public CreateModel(GameService gameService)
        {
            _gameService = gameService;
        }

        public void OnGet()
        {
        }

        // Rebre les dades quan l'usuari fa clic a "Guardar"
        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _gameService.Add(Game);
            return RedirectToPage("./Index");
        }
    }
}