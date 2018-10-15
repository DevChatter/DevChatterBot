using System;
using System.Collections.Generic;
using System.Threading;
using DevChatter.Bot.Core.Automation;
using DevChatter.Bot.Core.BotModules.VotingModule;
using DevChatter.Bot.Core.Data.Model;
using DevChatter.Bot.Core.Events.Args;
using DevChatter.Bot.Core.Systems.Chat;
using DevChatter.Bot.Core.Systems.Streaming;
using FluentAssertions;
using Moq;
using UnitTests.Fakes;
using Xunit;

namespace UnitTests.Core.BotModules.VotingModules
{
    public class EndVoteOperationShould
    {
        [Fact]
        public void SetIsVoteActiveToFalse_GivenVoteEnded()
        {
            var (operation, args, votingSystem, automationSystem) = GetTestOperation();

            operation.TryToExecute(args);

            votingSystem.IsVoteActive.Should().BeFalse();
        }

        [Fact]
        public void ScheduleEndOfVoting_GivenSpecifiedTimeLimit()
        {
            var (operation, args, _, automationSystem) = GetTestOperation();

            args.Arguments.Add("2s");

            operation.TryToExecute(args);
            IIntervalAction action = automationSystem.IntervalAction;

            action.Should().BeOfType<DelayedVoteEnd>();

            action.IsTimeToRun().Should().BeFalse();

            Thread.Sleep(TimeSpan.FromSeconds(2));

            action.IsTimeToRun().Should().BeTrue();
        }

        private static (EndVoteOperation, CommandReceivedEventArgs, VotingSystem, FakeActionSystem) GetTestOperation(ChatUser chatUser = null)
        {
            var mock = new Mock<IVotingDisplayNotification>();
            var votingSystem = new VotingSystem(mock.Object);
            votingSystem.StartVote(new List<string> {"A", "B", "C"});
            votingSystem.ApplyVote(new ChatUser{UserId = "Foo"}, "1", new Mock<IChatClient>().Object);
            var automationSystem = new FakeActionSystem();
            var endVoteOperation = new EndVoteOperation(votingSystem, automationSystem);

            var commandReceivedEventArgs = new CommandReceivedEventArgs
            {
                Arguments = new List<string> {"end"},
                ChatUser = chatUser ?? new ChatUser { Role = UserRole.Mod }
            };
            return (endVoteOperation, commandReceivedEventArgs, votingSystem, automationSystem);
        }
    }
}
