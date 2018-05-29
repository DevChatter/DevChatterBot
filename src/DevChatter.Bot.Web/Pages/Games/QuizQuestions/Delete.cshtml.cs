using DevChatter.Bot.Core.Data.Model;
using DevChatter.Bot.Infra.Ef;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace DevChatter.Bot.Web.Pages.QuizQuestions
{
    public class DeleteModel : PageModel
    {
        private readonly AppDataContext _context;

        public DeleteModel(AppDataContext context)
        {
            _context = context;
        }

        [BindProperty]
        public QuizQuestion QuizQuestion { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            QuizQuestion = await _context.QuizQuestions.FirstOrDefaultAsync(m => m.Id == id);

            if (QuizQuestion == null)
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

            QuizQuestion = await _context.QuizQuestions.FindAsync(id);

            if (QuizQuestion != null)
            {
                _context.QuizQuestions.Remove(QuizQuestion);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
