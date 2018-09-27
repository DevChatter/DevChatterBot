using DevChatter.Bot.Core.Data.Model;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using DevChatter.Bot.Infra.Ef;

namespace DevChatter.Bot.Web.Pages.ManageOverlay
{
    public class IndexModel : PageModel
    {
        private readonly AppDataContext _context;

        public IndexModel(AppDataContext context)
        {
            _context = context;
        }

        public IList<CanvasProperties> CanvasProperties { get;set; }

        public async Task OnGetAsync()
        {
            CanvasProperties = await _context.CanvasProperties.ToListAsync();
        }
    }
}
