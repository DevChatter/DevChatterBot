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

namespace UnitTests.Core.Commands.Operations.DeleteCommandOperationTests
{
    public class TryToExecuteShould
    {
        private readonly Mock<IRepository> _repositoryMock;
        private readonly IList<IBotCommand> _allCommands;
        private readonly SimpleCommand _simpleCommand;
        private readonly CommandReceivedEventArgs _commandReceivedEventArgs;

        public TryToExecuteShould()
        {
            _simpleCommand = new SimpleCommand("foo", "bar");

            _repositoryMock = new Mock<IRepository>();
            _repositoryMock.Setup(x => x.Single(It.IsAny<CommandPolicy>())).Returns(_simpleCommand);

            _allCommands = new List<IBotCommand>
            {
                _simpleCommand,
            };

            _commandReceivedEventArgs = new CommandReceivedEventArgs
            {
                Arguments = new List<string> {"remove", "foo"},
                CommandWord = "!commands",
            };
        }

        [Fact]
        public void NotRemoveSimpleCommandFromAllCommands_GivenUserIsAnyone()
        {
            var someRandomGuyFromChat = new ChatUser
            {
                Role = UserRole.Everyone
            };

            _commandReceivedEventArgs.ChatUser = someRandomGuyFromChat;
            var deleteCommand = new DeleteCommandOperation(_repositoryMock.Object, _allCommands);
            deleteCommand.TryToExecute(_commandReceivedEventArgs);
            _allCommands.Should().Contain(_simpleCommand);
        }

        [Fact]
        public void NotRemoveSimpleCommandFromRepository_GivenUserIsAnyone()
        {
            var someRandomGuyFromChat = new ChatUser
            {
                Role = UserRole.Everyone
            };

            _commandReceivedEventArgs.ChatUser = someRandomGuyFromChat;
            var deleteCommand = new DeleteCommandOperation(_repositoryMock.Object, _allCommands);
            deleteCommand.TryToExecute(_commandReceivedEventArgs);
            _repositoryMock.Verify(x => x.Remove(_simpleCommand), Times.Never);
        }

        [Fact]
        public void RemoveSimpleCommandFromAllCommands_GivenUserIsMod()
        {
            var someRandomGuyFromChat = new ChatUser
            {
                Role = UserRole.Mod
            };

            _commandReceivedEventArgs.ChatUser = someRandomGuyFromChat;
            var deleteCommand = new DeleteCommandOperation(_repositoryMock.Object, _allCommands);
            deleteCommand.TryToExecute(_commandReceivedEventArgs);
            _allCommands.Should().NotContain(_simpleCommand);
        }

        [Fact]
        public void RemoveSimpleCommandFromRepository_GivenUserIsMod()
        {
            var moderator = new ChatUser
            {
                Role = UserRole.Mod
            };

            _commandReceivedEventArgs.ChatUser = moderator;

            var deleteCommand = new DeleteCommandOperation(_repositoryMock.Object, _allCommands);
            deleteCommand.TryToExecute(_commandReceivedEventArgs);
            _repositoryMock.Verify(x => x.Remove(_simpleCommand), Times.Exactly(1));
        }
    }
}
