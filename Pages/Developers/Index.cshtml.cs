using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using VideoGameManager.Data;
using VideoGameManager.Models;

namespace VideoGameManager.Pages.Developers
{
    public class IndexModel : PageModel
    {
        private readonly GameStoreContext _context;

        public List<Developer> Developers { get; set; } = new();

        public IndexModel(GameStoreContext context) => _context = context;

        public async Task OnGet()
        {
            Developers = await _context.Developers.Include(d => d.Games).ToListAsync();
        }
    }
}