using DevChatter.Bot.Core.Data.Model;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DevChatter.Bot.Web.Pages.Hangman
{
    public class IndexModel : PageModel
    {
        private readonly DevChatter.Bot.Infra.Ef.AppDataContext _context;

        public IndexModel(DevChatter.Bot.Infra.Ef.AppDataContext context)
        {
            _context = context;
        }

        public IList<HangmanWord> HangmanWord { get;set; }

        public async Task OnGetAsync()
        {
            HangmanWord = await _context.HangmanWords.ToListAsync();
        }
    }
}
