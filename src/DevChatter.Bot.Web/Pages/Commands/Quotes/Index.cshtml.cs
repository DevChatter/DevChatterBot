using DevChatter.Bot.Core.Data.Model;
using DevChatter.Bot.Infra.Ef;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DevChatter.Bot.Web.Pages.Commands.Quotes
{
    public class IndexModel : PageModel
    {
        private readonly AppDataContext _context;

        public IndexModel(AppDataContext context)
        {
            _context = context;
        }

        public IList<QuoteEntity> QuoteEntity { get;set; }

        public async Task OnGetAsync()
        {
            QuoteEntity = await _context.QuoteEntities.ToListAsync();
        }
    }
}
