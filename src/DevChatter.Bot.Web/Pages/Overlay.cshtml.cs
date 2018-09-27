using System.Collections.Generic;
using DevChatter.Bot.Core.Data;
using DevChatter.Bot.Core.Data.Model;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DevChatter.Bot.Web.Pages
{
    public class OverlayModel : PageModel
    {
        private readonly IRepository _repository;

        public OverlayModel(IRepository repository)
        {
            _repository = repository;
        }

        public List<CanvasProperties> Canvases { get; set; }

        public void OnGet()
        {
            Canvases = _repository.List<CanvasProperties>();
        }
    }
}
