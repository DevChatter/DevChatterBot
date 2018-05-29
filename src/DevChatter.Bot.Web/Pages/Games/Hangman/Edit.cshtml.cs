using DevChatter.Bot.Core.Data.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DevChatter.Bot.Web.Pages.Games.Hangman
{
    public class EditModel : PageModel
    {
        private readonly DevChatter.Bot.Infra.Ef.AppDataContext _context;

        public EditModel(DevChatter.Bot.Infra.Ef.AppDataContext context)
        {
            _context = context;
        }

        [BindProperty]
        public HangmanWord HangmanWord { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            HangmanWord = await _context.HangmanWords.FirstOrDefaultAsync(m => m.Id == id);

            if (HangmanWord == null)
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

            _context.Attach(HangmanWord).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HangmanWordExists(HangmanWord.Id))
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

        private bool HangmanWordExists(Guid id)
        {
            return _context.HangmanWords.Any(e => e.Id == id);
        }
    }
}
