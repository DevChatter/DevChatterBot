using DevChatter.Bot.Core.Commands;
using DevChatter.Bot.Core.Commands.Operations;
using DevChatter.Bot.Core.Data;
using DevChatter.Bot.Core.Data.Model;
using DevChatter.Bot.Core.Data.Specifications;
using DevChatter.Bot.Core.Events.Args;
using FluentAssertions;
using Moq;
using System.Collections.Generic;
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
            var mockRepo = new Mock<IRepository>();
            mockRepo.Setup(repo => repo.Single(It.IsAny<SimpleCommandPolicy>()))
                .Returns(new SimpleCommand(commandWord, commandResponse));
            var botCommands = new List<IBotCommand>();

            var addCommandOperation = new AddCommandOperation(mockRepo.Object, botCommands);

            var message = addCommandOperation.TryToExecute(GetTestEventArgs(commandWord, commandResponse, "role", UserRole.Mod));

            mockRepo.Verify(x => x.Create(It.IsAny<SimpleCommand>()), Times.Never);
            message.Should().Contain(commandWord);
        }

        [Fact]
        public void NotSaveCommand_WhenUserIsNotModerator()
        {
            var mockRepo = new Mock<IRepository>();
            var botCommands = new List<IBotCommand>();

            var addCommandOperation = new AddCommandOperation(mockRepo.Object, botCommands);

            var message = addCommandOperation.TryToExecute(GetTestEventArgs("commandWord", "response", "role", UserRole.Everyone));

            mockRepo.Verify(x => x.Create(It.IsAny<SimpleCommand>()), Times.Never);
            message.Should().Be("You need to be a moderator to add a command.");
        }

        [Fact]
        public void SaveCommand_GivenValidArguments()
        {
            var newCommandword = "commandWord";
            var mockRepo = new Mock<IRepository>();
            var botCommands = new List<IBotCommand>();

            var addCommandOperation = new AddCommandOperation(mockRepo.Object, botCommands);

            var message = addCommandOperation.TryToExecute(GetTestEventArgs(newCommandword, "response", "role", UserRole.Mod));

            mockRepo.Verify(x => x.Create(It.IsAny<SimpleCommand>()), Times.Once);
            message.Should().Contain(newCommandword);
        }

        private static CommandReceivedEventArgs GetTestEventArgs(string newWord, string response,
            string roleRequired, UserRole userRole)
        {
            var commandReceivedEventArgs = new CommandReceivedEventArgs
            {
                Arguments = new[] {"add", newWord, response, roleRequired},
                ChatUser = new ChatUser {Role = userRole}
            };
            return commandReceivedEventArgs;
        }
    }
}
