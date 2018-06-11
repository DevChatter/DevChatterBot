using System.ComponentModel;
using System.Linq;
using DevChatter.Bot.Core;
using Hangfire;
using Hangfire.Server;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DevChatter.Bot.Web.Pages
{
    public class BotAdminModel : PageModel
    {
        public void OnGet()
        {

        }

        public void OnPost()
        {
            var runningJobs = JobStorage.Current.GetMonitoringApi()
                .ProcessingJobs(0, int.MaxValue).ToList();

            bool alreadyRunning = runningJobs.Any(j => j.Value.Job.Type == typeof(BotWorker));
            if (!alreadyRunning)
            {
                BackgroundJob.Enqueue<BotWorker>(bw => bw.Start());
            }
        }

        public string Message { get; set; }

        public class BotWorker : Worker
        {
            private readonly BotMain _botMain;

            public BotWorker(BotMain botMain)
            {
                _botMain = botMain;
            }

            [DisableConcurrentExecution(60)]
            public void Start()
            {
                _botMain.Run();

            }
        }
    }
}
