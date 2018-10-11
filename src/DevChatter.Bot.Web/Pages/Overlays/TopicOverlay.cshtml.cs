using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DevChatter.Bot.Web.Pages.Overlays
{
    public class TopicOverlayModel : PageModel
    {
        public void OnGet()
        {
            TopicText = "Coding in C# and JavaScript!";
        }

        public string TopicText { get; set; }
    }
}
