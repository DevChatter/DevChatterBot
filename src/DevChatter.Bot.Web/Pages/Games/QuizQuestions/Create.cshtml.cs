using System.Threading.Tasks;
using DevChatter.Bot.Core.Data.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DevChatter.Bot.Web.Pages.Games.QuizQuestions
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
        public QuizQuestion QuizQuestion { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.QuizQuestions.Add(QuizQuestion);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
