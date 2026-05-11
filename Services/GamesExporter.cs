using System.Text;
using System.Globalization;
using Microsoft.AspNetCore.Hosting;
using VideoGameManager.Models;

namespace VideoGameManager.Services
{
    public class GamesExporter
    {
        private readonly string _filePath;

        public GamesExporter(IWebHostEnvironment env)
        {
            string dataFolder = Path.Combine(env.ContentRootPath, "Data");
            if (!Directory.Exists(dataFolder)) Directory.CreateDirectory(dataFolder);

            _filePath = Path.Combine(dataFolder, "games.csv");
        }

        public byte[] ExportToCsv(IEnumerable<Game> games)
        {
            var sb = new StringBuilder();
            sb.AppendLine("Id,Title,Genre,Year,Score");
            foreach (var game in games)
            {
                string score = game.Score.ToString(CultureInfo.InvariantCulture);
                sb.AppendLine($"{game.Id},{game.Title},{game.Genre},{game.Year},{score}");
            }

            string csvContent = sb.ToString();
            File.WriteAllText(_filePath, csvContent, Encoding.UTF8); // ESCRITURA FÍSICA
            return Encoding.UTF8.GetBytes(csvContent);
        }

        public List<Game> ImportFromCsv()
        {
            var games = new List<Game>();
            if (!File.Exists(_filePath)) return games;
            var lines = File.ReadAllLines(_filePath);
            for (int i = 1; i < lines.Length; i++)
            {
                if (string.IsNullOrWhiteSpace(lines[i])) continue;
                var parts = lines[i].Split(',');
                if (parts.Length >= 5)
                {
                    games.Add(new Game
                    {
                        Id = int.Parse(parts[0]),
                        Title = parts[1],
                        Genre = parts[2],
                        Year = int.Parse(parts[3]),
                        Score = double.Parse(parts[4], CultureInfo.InvariantCulture)
                    });
                }
            }
            return games;
        }
    }
}