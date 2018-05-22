using DevChatter.Bot.Core.Data.Model;
using DevChatter.Bot.Infra.Ef;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DevChatter.Bot.Web.Pages.Quotes
{
    public class EditModel : PageModel
    {
        private readonly AppDataContext _context;

        public EditModel(AppDataContext context)
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

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(QuoteEntity).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!QuoteEntityExists(QuoteEntity.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool QuoteEntityExists(Guid id)
        {
            return _context.QuoteEntities.Any(e => e.Id == id);
        }
    }
}
