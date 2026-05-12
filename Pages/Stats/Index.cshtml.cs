using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VideoGameManager.Data;
using VideoGameManager.Models;

namespace VideoGameManager.Pages.Stats
{
    public class DecadeStat //Exercise 6.3
    {
        public int Decade { get; set; }
        public int Count { get; set; }
    }

    public class DeveloperStat //Exercice 7.1
    {
        public string Name { get; set; } = string.Empty;
        public int GameCount { get; set; }
        public double AvgScore { get; set; }
    }
    public class IndexModel : PageModel
    {
        public readonly GameStoreContext _context;
        public IndexModel(GameStoreContext context) => _context = context;

        public IList<Game> FilteredGames { get; set; } = new List<Game>();
        public IList<Game> TopRatedGames { get; set; } = new List<Game>();
        public IList<DecadeStat> GamesByDecade { get; set; } = new List<DecadeStat>();
        public IList<DeveloperStat> AvgByDeveloper { get; set; } = new List<DeveloperStat>();
        public IList<Developer> ProductiveDevs { get; set; } = new List<Developer>();


        [BindProperty(SupportsGet = true)]
        public string? TitleFilter { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? SelectedGenre { get; set; }

        [BindProperty(SupportsGet = true)]
        public int? MinYear { get; set; }

        [BindProperty(SupportsGet = true)]
        public int Threshold { get; set; } = 0;

        public SelectList Genres { get; set; }

        public async Task OnGetAsync()
        {
            var genreQuery = _context.Games
                .Select(x => x.Genre)
                .Distinct()
                .OrderBy(g => g);

            Genres = new SelectList(await genreQuery.ToListAsync());

            var query = _context.Games.Include(g => g.Developer).AsQueryable();

            if (!string.IsNullOrEmpty(SelectedGenre))
            {
                query = query.Where(x => x.Genre == SelectedGenre);
            }

            FilteredGames = await query.OrderByDescending(x => x.Score).ToListAsync();

            TopRatedGames = await _context.Games
                .Include(g => g.Developer)
                .OrderByDescending(g => g.Score)
                .Take(5)
                .ToListAsync();

            GamesByDecade = await _context.Games
                .GroupBy(g => (g.Year / 10) * 10)
                .Select(grp => new DecadeStat
                {
                    Decade = grp.Key,
                    Count = grp.Count()
                })
                .ToListAsync();

            AvgByDeveloper = await _context.Developers
                .Where(d => d.Games.Any())
                .Select(d => new DeveloperStat
                {
                    Name = d.Name,
                    GameCount = d.Games.Count,
                    AvgScore = d.Games.Average(g => g.Score)
                })
                .OrderByDescending(x => x.AvgScore)
                .ToListAsync();

            if (!string.IsNullOrWhiteSpace(TitleFilter))
                query = query.Where(g => g.Title.Contains(TitleFilter));

            if (!string.IsNullOrWhiteSpace(SelectedGenre))
                query = query.Where(g => g.Genre == SelectedGenre);

            if (MinYear.HasValue)
                query = query.Where(g => g.Year >= MinYear.Value);

            FilteredGames = await query.OrderBy(g => g.Title).ToListAsync();

            ProductiveDevs = await _context.Developers
                .Include(d => d.Games)
                .Where(d => d.Games.Count > Threshold)
                .OrderByDescending(d => d.Games.Count)
                .ToListAsync();

        }
    }
}
