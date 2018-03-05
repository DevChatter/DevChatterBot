using DevChatter.Bot.Core;
using DevChatter.Bot.Infra.Twitch;
using TwitchLib.Events.Client;
using TwitchLib.Models.Client;
using Xunit;

namespace UnitTests.Twitch.EventArgsExtensionsTests
{
    public class ToCommandReceivedEventArgsShould
    {
        [Fact]
        public void MapCommandText()
        {
            // TODO: Replace with a builder structure that doesn't require making the object
            //var fullIrcString = "lurk";
            //ChatCommand chatCommand = GetTestCommand(fullIrcString);
            //var srcArgs = new OnChatCommandReceivedArgs
            //{
            //    Command = chatCommand
            //};

            //CommandReceivedEventArgs resultArgs = srcArgs.ToCommandReceivedEventArgs();

            //Assert.Equal(fullIrcString, resultArgs.CommandWord);
        }

        private static ChatCommand GetTestCommand(string fullIrcString)
        {
            var messageEmoteCollection = new MessageEmoteCollection();
            return new ChatCommand(fullIrcString, new ChatMessage("botName", fullIrcString, ref messageEmoteCollection));
        }
    }
}