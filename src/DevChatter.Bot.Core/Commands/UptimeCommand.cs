using DevChatter.Bot.Core.Data;
using DevChatter.Bot.Core.Data.Model;
using DevChatter.Bot.Core.Events.Args;
using DevChatter.Bot.Core.Systems.Chat;
using DevChatter.Bot.Core.Systems.Streaming;
using System;
using DevChatter.Bot.Core.Util;

namespace DevChatter.Bot.Core.Commands
{
    public class UptimeCommand : BaseCommand
    {
        private readonly IStreamingPlatform _streamingPlatform;
        private readonly ILoggerAdapter<UptimeCommand> _logger;

        public UptimeCommand(IRepository repository, IStreamingPlatform streamingPlatform,
            ILoggerAdapter<UptimeCommand> logger)
            : base(repository, UserRole.Everyone)
        {
            _streamingPlatform = streamingPlatform;
            _logger = logger;
            HelpText = "Just type \"!uptime\" and it will tell you how long we've been streaming.";
        }

        protected override void HandleCommand(IChatClient chatClient, CommandReceivedEventArgs eventArgs)
        {
            try
            {
                TimeSpan? timeSpan = _streamingPlatform.GetUptimeAsync().Result;
                string message = timeSpan.HasValue
                    ? $"The stream has been going for {timeSpan:hh\\:mm\\:ss}"
                    : "Something's wrong. Are we live right now?";
                chatClient.SendMessage(message);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Failed trying to get UpTime data.");
            }
        }
    }
}
