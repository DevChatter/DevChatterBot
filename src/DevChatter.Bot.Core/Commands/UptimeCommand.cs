using System;
using DevChatter.Bot.Core.Data.Model;
using DevChatter.Bot.Core.Events;
using DevChatter.Bot.Core.Systems.Chat;
using DevChatter.Bot.Core.Systems.Streaming;

namespace DevChatter.Bot.Core.Commands
{
    public class UptimeCommand : BaseCommand
    {
        private readonly StreamingPlatform _streamingPlatform;

        public UptimeCommand(StreamingPlatform streamingPlatform)
            : base("Uptime", UserRole.Everyone)
        {
            _streamingPlatform = streamingPlatform;
            HelpText = "Just type \"!uptime\" and it will tell you how long we've been streaming.";
        }

        public override void Process(IChatClient chatClient, CommandReceivedEventArgs eventArgs)
        {
            TimeSpan? timeSpan = _streamingPlatform.GetUptimeAsync().Result;
            if (timeSpan.HasValue)
            {
                chatClient.SendMessage($"The stream has been going for {timeSpan:hh\\:mm\\:ss}");
            }
            else
            {
                chatClient.SendMessage("Something's Wrong. Are we live right now?");
            }
        }
    }
}