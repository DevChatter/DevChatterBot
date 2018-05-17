using DevChatter.Bot.Core.Data.Model;
using DevChatter.Bot.Infra.Ef;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace DevChatter.Bot.Web.Pages.Quotes
{
    public class DetailsModel : PageModel
    {
        private readonly AppDataContext _context;

        public DetailsModel(AppDataContext context)
        {
            _context = context;
        }

        public QuoteEntity QuoteEntity { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            QuoteEntity = await _context.QuoteEntities.FirstOrDefaultAsync(m => m.Id == id);

            if (QuoteEntity == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
