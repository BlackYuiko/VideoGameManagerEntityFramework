using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Hosting;
using VideoGameManager.Services;
using System.IO;
using System;

namespace VideoGameManager.Pages.Files
{
    public class IndexModel : PageModel
    {
        private readonly IWebHostEnvironment _env;
        private readonly GameService _gameService;
        private readonly GamesExporter _gamesExporter;
        private readonly RankingExporter _rankingExporter;

        public string[] LogEntries { get; set; } = Array.Empty<string>();

        public IndexModel(IWebHostEnvironment env, GameService gameService, GamesExporter gamesExporter, RankingExporter rankingExporter)
        {
            _env = env;
            _gameService = gameService;
            _gamesExporter = gamesExporter;
            _rankingExporter = rankingExporter;
        }

        public void OnGet()
        {
            string logPath = Path.Combine(_env.ContentRootPath, "Data", "activity_log.txt");

            if (System.IO.File.Exists(logPath))
            {
                LogEntries = System.IO.File.ReadAllLines(logPath);
            }
        }

        public IActionResult OnPostExportJson()
        {
            return RedirectToPage();
        }

        public IActionResult OnPostImportJson()
        {
            return RedirectToPage();
        }

        public IActionResult OnPostExportCsv()
        {
            var games = _gameService.GetAll();
            byte[] fileBytes = _gamesExporter.ExportToCsv(games);
            return File(fileBytes, "text/csv", "games.csv");
        }

        public IActionResult OnPostImportCsv()
        {
            var gamesFromCsv = _gamesExporter.ImportFromCsv();
            if (gamesFromCsv.Any())
            {
                _gameService.OverwriteGames(gamesFromCsv);
            }
            return RedirectToPage();
        }

        public IActionResult OnPostExportXml()
        {
            _rankingExporter.ExportRankingToXml(_gameService.GetAll());
            return RedirectToPage();
        }
    }
}