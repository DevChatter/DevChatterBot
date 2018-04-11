using DevChatter.Bot.Core.Commands;
using DevChatter.Bot.Core.Events;
using DevChatter.Bot.Core.Events.Args;
using DevChatter.Bot.Core.Systems.Chat;
using Moq;
using UnitTests.Fakes;
using Xunit;

namespace UnitTests.Core.Commands.SimpleCommandTests
{
    public class ProcessShould
    {
        [Fact]
        public void SendStaticMessage_GivenNoTokensInMessage()
        {
            var staticResponse = "Hello";
            var simpleCommand = new SimpleCommand(null, staticResponse);

            var mockChatClient = new Mock<IChatClient>();

            simpleCommand.Process(mockChatClient.Object, new CommandReceivedEventArgs());

            mockChatClient.Verify(x => x.SendMessage(staticResponse));
        }

        [Fact]
        public void SendHydratedMessage_GivenTokensInMessage()
        {
            var simpleCommand = new SimpleCommand(null, "[UserDisplayName] says hello!");

            var mockChatClient = new Mock<IChatClient>();

            var commandReceivedEventArgs = new CommandReceivedEventArgs {ChatUser = {DisplayName = "Brendan"}};

            simpleCommand.Process(mockChatClient.Object, commandReceivedEventArgs);

            mockChatClient.Verify(x => x.SendMessage("Brendan says hello!"));
        }
    }
}