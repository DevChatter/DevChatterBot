using DevChatter.Bot.Core.Commands;
using DevChatter.Bot.Core.Events;
using UnitTests.Fakes;
using Xunit;

namespace UnitTests.Core.Commands.SimpleCommandTests
{
    public class ProcessShould
    {
        [Fact]
        public void SendStaticMessage_GivenNoTokensInMessage()
        {
            var simpleCommand = new SimpleCommand(null, "Hello");

            var fakeChatClient = new FakeChatClient();

            simpleCommand.Process(fakeChatClient, new CommandReceivedEventArgs());

            Assert.Equal("Hello", fakeChatClient.SentMessage);
        }

        [Fact]
        public void SendHydratedMessage_GivenTokensInMessage()
        {
            var simpleCommand = new SimpleCommand(null, "[UserDisplayName] says hello!");
            var fakeChatClient = new FakeChatClient();
            var commandReceivedEventArgs = new CommandReceivedEventArgs {ChatUser = {DisplayName = "Brendan"}};

            simpleCommand.Process(fakeChatClient, commandReceivedEventArgs);

            Assert.Equal("Brendan says hello!", fakeChatClient.SentMessage);
        }
    }
}