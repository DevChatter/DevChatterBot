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

namespace UnitTests.Core.Commands.DeleteCommandTests
{

    public class DeleteCommandShould
    {
        private readonly Mock<IRepository> _RepositoryMock = new Mock<IRepository>();
        private readonly IList<IBotCommand> _AllCommands = new List<IBotCommand>();
        private readonly SimpleCommand _QuoteCommand;
        private readonly CommandReceivedEventArgs _CommandReceivedEventArgs;

        public DeleteCommandShould()
        {
            _QuoteCommand = new SimpleCommand("foo", "bar");
            var botCommand = new QuoteCommand(_RepositoryMock.Object)
            {
                CommandWords = { "foo"}
            };

            _RepositoryMock.Setup(x => x.Single(It.IsAny<CommandPolicy>())).Returns(_QuoteCommand);
            _AllCommands.Add(botCommand);

            _CommandReceivedEventArgs = new CommandReceivedEventArgs
            {
                Arguments = new List<string> {"remove", "foo"},
                CommandWord = "!commands",
                ChatUser = new ChatUser
                {
                    Role = UserRole.Mod
                }
            };
        }

        [Fact]
        public void RemoveSimpleCommandFromRepository_GivenUserIsMod()
        {
            var deleteCommand = new DeleteCommandOperation(_RepositoryMock.Object, _AllCommands);
            deleteCommand.TryToExecute(_CommandReceivedEventArgs);
            _RepositoryMock.Verify(x => x.Remove(_QuoteCommand), Times.Exactly(1));
        }

        [Fact]
        public void RemoveSimpleCommandFromAllCommands_GivenUserIsMod()
        {
            var deleteCommand = new DeleteCommandOperation(_RepositoryMock.Object, _AllCommands);
            deleteCommand.TryToExecute(_CommandReceivedEventArgs);
            _AllCommands.Should().NotContain(_QuoteCommand);
        }
    }
}
