using DevChatter.Bot.Core.Events;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DevChatter.Bot.Web.Pages
{
    public class IndexModel : PageModel
    {
        public CommandHandlerSettings CommandHandlerSettings { get; private set; }

        public void OnGet()
        {

        }
    }
}
