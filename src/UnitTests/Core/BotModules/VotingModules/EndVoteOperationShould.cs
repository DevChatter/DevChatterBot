using System.Collections.Generic;
using DevChatter.Bot.Core.BotModules.VotingModule;
using DevChatter.Bot.Core.Data.Model;
using DevChatter.Bot.Core.Events.Args;
using DevChatter.Bot.Core.Systems.Chat;
using DevChatter.Bot.Core.Systems.Streaming;
using FluentAssertions;
using Moq;
using Xunit;

namespace UnitTests.Core.BotModules.VotingModules
{
    public class EndVoteOperationShould
    {
        [Fact]
        public void SetIsVoteActiveToFalse_GivenVoteEnded()
        {
            var (operation, args, votingSystem) = GetTestOperation();

            operation.TryToExecute(args);

            votingSystem.IsVoteActive.Should().BeFalse();
        }

        private static (EndVoteOperation, CommandReceivedEventArgs, VotingSystem) GetTestOperation(ChatUser chatUser = null)
        {
            var mock = new Mock<IVotingDisplayNotification>();
            var votingSystem = new VotingSystem(mock.Object);
            votingSystem.StartVote(new List<string> {"A", "B", "C"});
            votingSystem.ApplyVote(new ChatUser{UserId = "Foo"}, "1", new Mock<IChatClient>().Object);
            var endVoteOperation = new EndVoteOperation(votingSystem);

            var commandReceivedEventArgs = new CommandReceivedEventArgs
            {
                Arguments = new List<string> {"end"},
                ChatUser = chatUser ?? new ChatUser { Role = UserRole.Mod }
            };
            return (endVoteOperation, commandReceivedEventArgs, votingSystem);
        }
    }
}
