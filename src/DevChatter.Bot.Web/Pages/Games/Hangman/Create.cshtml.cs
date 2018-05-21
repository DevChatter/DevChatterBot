using DevChatter.Bot.Core.Data.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace DevChatter.Bot.Web.Pages.Games.Hangman
{
    public class CreateModel : PageModel
    {
        private readonly DevChatter.Bot.Infra.Ef.AppDataContext _context;

        public CreateModel(DevChatter.Bot.Infra.Ef.AppDataContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public HangmanWord HangmanWord { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.HangmanWords.Add(HangmanWord);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
