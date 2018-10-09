using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DevChatter.Bot.Web.Pages.Overlays
{
    public class TopicOverlayModel : PageModel
    {
        public void OnGet()
        {

        }

        public string TopicText { get; set; }
    }
}
