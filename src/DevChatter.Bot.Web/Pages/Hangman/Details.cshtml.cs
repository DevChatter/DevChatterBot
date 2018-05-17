using DevChatter.Bot.Core.Data.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace DevChatter.Bot.Web.Pages.Hangman
{
    public class DetailsModel : PageModel
    {
        private readonly DevChatter.Bot.Infra.Ef.AppDataContext _context;

        public DetailsModel(DevChatter.Bot.Infra.Ef.AppDataContext context)
        {
            _context = context;
        }

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
    }
}
