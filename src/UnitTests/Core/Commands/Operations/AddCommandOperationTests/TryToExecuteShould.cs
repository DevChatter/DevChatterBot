using System.Collections.Generic;
using DevChatter.Bot.Core.Commands;
using DevChatter.Bot.Core.Commands.Operations;
using DevChatter.Bot.Core.Data;
using DevChatter.Bot.Core.Data.Model;
using DevChatter.Bot.Core.Data.Specifications;
using DevChatter.Bot.Core.Events.Args;
using FluentAssertions;
using Moq;
using Xunit;

namespace UnitTests.Core.Commands.Operations.AddCommandOperationTests
{
    public class TryToExecuteShould
    {
        [Fact]
        public void NotSaveCommand_GivenAlreadyRegisteredCommandText()
        {
            var commandResponse = "responseText";
            var commandWord = "commandWord";
            var commandReceivedEventArgs = new CommandReceivedEventArgs
            {
                Arguments = new[] {"add", commandWord, commandResponse, "roleRequired"},
                ChatUser = new ChatUser {Role = UserRole.Mod}
            };
            var mockRepo = new Mock<IRepository>();
            mockRepo.Setup(repo => repo.Single(It.IsAny<CommandPolicy>()))
                .Returns(new SimpleCommand(commandWord, commandResponse));
            var botCommands = new List<IBotCommand>();

            var addCommandOperation = new AddCommandOperation(mockRepo.Object, botCommands);

            var message = addCommandOperation.TryToExecute(commandReceivedEventArgs);

            mockRepo.Verify(x => x.Create(It.IsAny<SimpleCommand>()), Times.Never);
            message.Should().Contain(commandWord);
        }

        [Fact]
        public void NotSaveCommand_WhenUserIsNotModerator()
        {
            var commandReceivedEventArgs = new CommandReceivedEventArgs
            {
                Arguments = new[] {"add", "commandWord", "responseText", "roleRequired"},
                ChatUser = new ChatUser {Role = UserRole.Everyone}
            };
            var mockRepo = new Mock<IRepository>();
            var botCommands = new List<IBotCommand>();

            var addCommandOperation = new AddCommandOperation(mockRepo.Object, botCommands);

            var message = addCommandOperation.TryToExecute(commandReceivedEventArgs);

            mockRepo.Verify(x => x.Create(It.IsAny<SimpleCommand>()), Times.Never);
            message.Should().Be("You need to be a moderator to add a command.");
        }

        [Fact]
        public void SaveCommand_GivenValidArguments()
        {
            var newCommandword = "commandWord";
            var commandReceivedEventArgs = new CommandReceivedEventArgs
            {
                Arguments = new[] {"add", newCommandword, "responseText", "roleRequired"},
                ChatUser = new ChatUser {Role = UserRole.Mod}
            };
            var mockRepo = new Mock<IRepository>();
            var botCommands = new List<IBotCommand>();

            var addCommandOperation = new AddCommandOperation(mockRepo.Object, botCommands);

            var message = addCommandOperation.TryToExecute(commandReceivedEventArgs);

            mockRepo.Verify(x => x.Create(It.IsAny<SimpleCommand>()), Times.Once);
            message.Should().Contain(newCommandword);
        }
    }
}
