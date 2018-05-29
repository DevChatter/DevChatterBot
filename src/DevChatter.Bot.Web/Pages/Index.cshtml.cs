using System.Collections.Generic;
using System.Linq;
using DevChatter.Bot.Core.Events;
using DevChatter.Bot.Infra.Ef;
using DevChatter.Bot.Web.ViewModels;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DevChatter.Bot.Web.Pages
{
    public class IndexModel : PageModel
    {
        private readonly AppDataContext _context;

        public IndexModel(AppDataContext context)
        {
            _context = context;
        }

        public CommandHandlerSettings CommandHandlerSettings { get; private set; }

        public IList<ScheduleViewModel> ScheduleViewModels { get; set; }

        public void OnGet()
        {
            ScheduleViewModels = _context.ScheduleEntities.Select(ScheduleViewModel.FromScheduleEntity).ToList();
        }
    }
}
