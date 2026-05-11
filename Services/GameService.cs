using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using VideoGameManager.Models;

namespace VideoGameManager.Services
{
    public class GameService
    {
        private readonly List<Game> _games;
        private int _nextId;
        private readonly string _logFilePath;
        private readonly GameRepository _gameRepository;
        private readonly GamesExporter _gamesExporter;

        public GameService(IWebHostEnvironment webHostEnvironment, GameRepository gameRepository, GamesExporter gamesExporter)
        {
            _gameRepository = gameRepository;
            _gamesExporter = gamesExporter;

            // Usamos ContentRootPath para que el TXT esté con el JSON y el CSV
            string dataFolder = Path.Combine(webHostEnvironment.ContentRootPath, "Data");

            if (!Directory.Exists(dataFolder))
            {
                Directory.CreateDirectory(dataFolder);
            }

            _logFilePath = Path.Combine(dataFolder, "activity_log.txt");

            // Carga inicial de datos
            _games = _gameRepository.LoadAll();
            _nextId = _games.Any() ? _games.Max(g => g.Id) + 1 : 1;
        }

        private void LogAction(string action, string gameTitle)
        {
            string timestamp = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            string logLine = $"[{timestamp}] [{action}] {gameTitle}{Environment.NewLine}";
            File.AppendAllText(_logFilePath, logLine);
        }

        private void AutoSave()
        {
            _gameRepository.SaveAll(_games);
            _gamesExporter.ExportToCsv(_games);
        }

        public List<Game> GetAll() => _games;

        public Game? GetById(int id) => _games.FirstOrDefault(g => g.Id == id);

        public void Add(Game game)
        {
            game.Id = _nextId++;
            _games.Add(game);
            LogAction("CREATE", game.Title);
            AutoSave();
        }

        public void Update(Game updatedGame)
        {
            var existingGame = _games.FirstOrDefault(g => g.Id == updatedGame.Id);
            if (existingGame != null)
            {
                existingGame.Title = updatedGame.Title;
                existingGame.Genre = updatedGame.Genre;
                existingGame.Year = updatedGame.Year;
                existingGame.Score = updatedGame.Score;
                existingGame.Description = updatedGame.Description;

                LogAction("UPDATE", updatedGame.Title);
                AutoSave();
            }
        }

        public void Delete(int id)
        {
            var game = _games.FirstOrDefault(g => g.Id == id);
            if (game != null)
            {
                string title = game.Title;
                _games.Remove(game);
                LogAction("DELETE", title);
                AutoSave();
            }
        }

        public void OverwriteGames(List<Game> newGames)
        {
            _games.Clear();
            _games.AddRange(newGames);
            _nextId = _games.Any() ? _games.Max(g => g.Id) + 1 : 1;
            AutoSave();
        }
    }
}