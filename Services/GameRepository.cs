using System.Text.Json;
using Microsoft.AspNetCore.Hosting;
using VideoGameManager.Models;

namespace VideoGameManager.Services
{
    public class GameRepository
    {
        private readonly string _filePath;

        public GameRepository(IWebHostEnvironment env)
        {
            // Usamos ContentRootPath para que se guarde en la raíz del proyecto
            string dataFolder = Path.Combine(env.ContentRootPath, "Data");
            if (!Directory.Exists(dataFolder)) Directory.CreateDirectory(dataFolder);

            _filePath = Path.Combine(dataFolder, "games.json");
        }

        public void SaveAll(IEnumerable<Game> games)
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            string json = JsonSerializer.Serialize(games, options);
            File.WriteAllText(_filePath, json);
        }

        public List<Game> LoadAll()
        {
            if (!File.Exists(_filePath)) return new List<Game>();
            string json = File.ReadAllText(_filePath);
            return JsonSerializer.Deserialize<List<Game>>(json) ?? new List<Game>();
        }
    }
}