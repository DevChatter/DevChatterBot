using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using DevChatter.Bot.Core.Data.Model;
using DevChatter.Bot.Infra.Ef;

namespace DevChatter.Bot.Web.Pages.ManageOverlay
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
        public CanvasProperties CanvasProperties { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.CanvasProperties.Add(CanvasProperties);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}