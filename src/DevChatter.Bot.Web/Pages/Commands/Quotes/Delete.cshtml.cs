using DevChatter.Bot.Core.Data.Model;
using DevChatter.Bot.Infra.Ef;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace DevChatter.Bot.Web.Pages.Commands.Quotes
{
    public class DeleteModel : PageModel
    {
        private readonly AppDataContext _context;

        public DeleteModel(AppDataContext context)
        {
            _context = context;
        }

        [BindProperty]
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

        public async Task<IActionResult> OnPostAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            QuoteEntity = await _context.QuoteEntities.FindAsync(id);

            if (QuoteEntity != null)
            {
                _context.QuoteEntities.Remove(QuoteEntity);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
