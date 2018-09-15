using DevChatter.Bot.Core.Commands;
using DevChatter.Bot.Core.Data;
using DevChatter.Bot.Core.Data.Model;
using DevChatter.Bot.Core.Data.Specifications;
using DevChatter.Bot.Core.Events.Args;
using DevChatter.Bot.Core.Systems.Chat;
using DevChatter.Bot.Core.Systems.Streaming;
using Moq;
using System.Collections.Generic;
using Xunit;

namespace UnitTests.Core.Commands.BlastCommandTests
{
    public class HandleCommandShould
    {
        [Fact]
        public void SendHelpText_GivenNoArguments()
        {
            var (chatClient, _, _, blastCommand) = GetTestCommandAndMocks();

            blastCommand.Process(chatClient.Object, new CommandReceivedEventArgs());

            chatClient.Verify(x => x.SendMessage(blastCommand.HelpText));
        }

        [Fact]
        public void SendMessage_GivenValidArgument()
        {
            var blast = new BlastTypeEntity
            {
                Name = "Foo", Message = "Bar", ImagePath = "Fizz"
            };
            var (chat, repo, display, command) = GetTestCommandAndMocks();
            repo.Setup(x => x.Single(It.IsAny<ISpecification<BlastTypeEntity>>()))
                .Returns(blast);

            command.Process(chat.Object, new CommandReceivedEventArgs
                { Arguments = new List<string>{blast.Name}});

            chat.Verify(x => x.SendMessage(blast.Message));
            display.Verify(x => x.Blast(blast.ImagePath));
        }

        private static (Mock<IChatClient> chatClient, Mock<IRepository> repository, Mock<IAnimationDisplayNotification> display, BlastCommand command) GetTestCommandAndMocks()
        {
            var chatClient = new Mock<IChatClient>();
            var repository = new Mock<IRepository>();
            var displayNotification = new Mock<IAnimationDisplayNotification>();
            var blastCommand = new BlastCommand(repository.Object, displayNotification.Object);
            return (chatClient, repository, displayNotification, blastCommand);
        }
    }
}
