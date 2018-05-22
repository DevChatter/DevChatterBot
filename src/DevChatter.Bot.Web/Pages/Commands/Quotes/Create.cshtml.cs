using DevChatter.Bot.Core.Data.Model;
using DevChatter.Bot.Infra.Ef;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace DevChatter.Bot.Web.Pages.Quotes
{
    public class CreateModel : PageModel
    {
        private readonly AppDataContext _context;

        public CreateModel(AppDataContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public QuoteEntity QuoteEntity { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.QuoteEntities.Add(QuoteEntity);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
