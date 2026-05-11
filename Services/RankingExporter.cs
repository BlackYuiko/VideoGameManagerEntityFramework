using System.Xml.Linq;
using Microsoft.AspNetCore.Hosting;
using VideoGameManager.Models;

namespace VideoGameManager.Services
{
    public class RankingExporter
    {
        private readonly string _filePath;

        public RankingExporter(IWebHostEnvironment env)
        {
            string dataFolder = Path.Combine(env.ContentRootPath, "Data");
            if (!Directory.Exists(dataFolder)) Directory.CreateDirectory(dataFolder);
            _filePath = Path.Combine(dataFolder, "ranking.xml");
        }

        public void ExportRankingToXml(IEnumerable<Game> games)
        {
            var sortedGames = games.OrderByDescending(g => g.Score).ToList();
            var doc = new XDocument(
                new XDeclaration("1.0", "utf-8", "yes"),
                new XElement("AppConfig",
                    new XElement("AppTitle", "VideoGame Ranking"),
                    new XElement("Games",
                        sortedGames.Select(g => new XElement("Game",
                            new XElement("id", g.Id),
                            new XElement("score", g.Score),
                            new XElement("title", g.Title),
                            new XElement("genre", g.Genre),
                            new XElement("year", g.Year)
                        ))
                    )
                )
            );
            doc.Save(_filePath);
        }
    }
}