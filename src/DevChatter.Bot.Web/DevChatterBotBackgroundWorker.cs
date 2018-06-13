using DevChatter.Bot.Core.Util;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System.Threading;
using System.Threading.Tasks;
using DevChatter.Bot.Core;

namespace DevChatter.Bot.Web
{
    public class DevChatterBotBackgroundWorker : BackgroundService
    {
        private readonly ILoggerAdapter<DevChatterBotBackgroundWorker> _logger;
        private readonly BotMain _botMain;
        private readonly BotConfiguration _settings;

        public DevChatterBotBackgroundWorker(IOptions<BotConfiguration> settings,
            ILoggerAdapter<DevChatterBotBackgroundWorker> logger, BotMain botMain)
        {
            _settings = settings.Value;
            _logger = logger;
            _botMain = botMain;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation($"DevChatterBotBackgroundWorker is starting.");

            stoppingToken.Register(() =>
                _logger.LogInformation($" DevChatterBotBackgroundWorker is stopping."));

            //while (!stoppingToken.IsCancellationRequested)
            //{
            //    _logger.LogInformation($"DevChatterBotBackgroundWorker doing background work.");

            //    await Task.Delay(_settings.CheckUpdateTime, stoppingToken);
            //}

            _logger.LogInformation($"DevChatterBotBackgroundWorker is stopping.");
        }

        public override async Task StopAsync(CancellationToken stoppingToken)
        {
            await _botMain.Stop();
        }
    }

}
