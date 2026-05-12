using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using VideoGameManager.Data;
using VideoGameManager.Models;

namespace VideoGameManager.Pages.Games
{
    public class IndexModel : PageModel
    {
        private readonly GameStoreContext _context;
        public IndexModel(GameStoreContext context) => _context = context;

        public List<Game> Games { get; set; } = new List<Game>();

        public async Task OnGet()
        {
            if (!_context.Developers.Any())
            {
                var dev1 = new Developer { Name = "Nintendo", Country = "Japan", FoundedYear = 1889 };
                var dev2 = new Developer { Name = "CD Projekt Red", Country = "Poland", FoundedYear = 1994 };
                _context.Developers.AddRange(dev1, dev2);
                _context.SaveChanges();

                _context.Games.AddRange(
                    new Game
                    {
                        Title = "The Legend of Zelda: TotK",
                        Genre = "Adventure",
                        Year = 2023,
                        Score = 9.8,
                        DeveloperId = dev1.Id
                    },
                    new Game
                    {
                        Title = "Mario Kart 8",
                        Genre = "Racing",
                        Year = 2014,
                        Score = 8.7,
                        DeveloperId = dev1.Id
                    },
                    new Game
                    {
                        Title = "The Witcher 3",
                        Genre = "RPG",
                        Year = 2015,
                        Score = 9.5,
                        DeveloperId = dev2.Id
                    }
                );
                _context.SaveChanges();
            }

            Games = await _context.Games.Include(g => g.Developer).ToListAsync();
        }
    }
}
