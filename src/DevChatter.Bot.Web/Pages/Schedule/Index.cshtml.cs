using DevChatter.Bot.Web.ViewModels;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;

namespace DevChatter.Bot.Web.Pages.Schedule
{
    public class IndexModel : PageModel
    {
        private readonly DevChatter.Bot.Infra.Ef.AppDataContext _context;

        public IndexModel(DevChatter.Bot.Infra.Ef.AppDataContext context)
        {
            _context = context;
        }

        public IList<ScheduleViewModel> ScheduleEntity { get;set; }

        public void OnGet()
        {
            ScheduleEntity = _context.ScheduleEntities.Select(ScheduleViewModel.FromScheduleEntity).ToList();
        }
    }
}
