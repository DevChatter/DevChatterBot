using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DevChatter.Bot.Core.Data.Model;
using DevChatter.Bot.Infra.Ef;

namespace DevChatter.Bot.Web.Pages.ManageOverlay
{
    public class DeleteModel : PageModel
    {
        private readonly DevChatter.Bot.Infra.Ef.AppDataContext _context;

        public DeleteModel(DevChatter.Bot.Infra.Ef.AppDataContext context)
        {
            _context = context;
        }

        [BindProperty]
        public CanvasProperties CanvasProperties { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            CanvasProperties = await _context.CanvasProperties.FirstOrDefaultAsync(m => m.Id == id);

            if (CanvasProperties == null)
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

            CanvasProperties = await _context.CanvasProperties.FindAsync(id);

            if (CanvasProperties != null)
            {
                _context.CanvasProperties.Remove(CanvasProperties);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
