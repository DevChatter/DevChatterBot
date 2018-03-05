using DevChatter.Bot.Core;
using TwitchLib.Events.Client;

namespace DevChatter.Bot.Infra.Twitch
{
    public static class EventArgsExtensions
    {
        public static CommandReceivedEventArgs ToCommandReceivedEventArgs(this OnChatCommandReceivedArgs src)
        {
            var commandReceivedEventArgs = new CommandReceivedEventArgs
            {
                CommandWord = src.Command.CommandText,
            };

            return commandReceivedEventArgs;
        }
    }
}