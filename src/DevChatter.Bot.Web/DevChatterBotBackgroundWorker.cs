using DevChatter.Bot.Core;
using DevChatter.Bot.Core.Automation;
using DevChatter.Bot.Core.Util;
using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;

namespace DevChatter.Bot.Web
{
    public class DevChatterBotBackgroundWorker : IHostedService
    {
        private readonly ILoggerAdapter<DevChatterBotBackgroundWorker> _logger;
        private readonly IAutomatedActionSystem _automatedActionSystem;
        private readonly BotMain _botMain;

        public DevChatterBotBackgroundWorker(BotMain botMain,
            IAutomatedActionSystem automatedActionSystem,
            ILoggerAdapter<DevChatterBotBackgroundWorker> logger)
        {
            _logger = logger;
            _automatedActionSystem = automatedActionSystem;
            _botMain = botMain;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("DevChatterBotBackgroundWorker StartAsync");
            return _botMain.Run();
        }

        public Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("DevChatterBotBackgroundWorker StopAsync");
            return _botMain.Stop();
        }
    }

}
