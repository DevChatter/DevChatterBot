using System;
using DevChatter.Bot.Core.Data.Model;
using DevChatter.Bot.Core.Events;
using DevChatter.Bot.Core.Systems.Chat;
using DevChatter.Bot.Core.Systems.Streaming;

namespace DevChatter.Bot.Core.Commands
{
    public class UptimeCommand : IBotCommand
    {
        private readonly StreamingPlatform _streamingPlatform;
        public UserRole RoleRequired { get; }
        public string CommandText { get; }
        public string HelpText { get; }
        public bool IsEnabled { get; }

        public UptimeCommand(StreamingPlatform streamingPlatform)
        {
            _streamingPlatform = streamingPlatform;
            RoleRequired = UserRole.Everyone;
            CommandText = "Uptime";
            HelpText = "Just type \"!uptime\" and it will tell you how long we've been streaming.";
            IsEnabled = true;
        }

        public void Process(IChatClient chatClient, CommandReceivedEventArgs eventArgs)
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