using DevChatter.Bot.Core.Automation;
using DevChatter.Bot.Core.BotModules.VotingModule;
using DevChatter.Bot.Core.Data;
using DevChatter.Bot.Core.Data.Model;
using DevChatter.Bot.Core.Events.Args;
using DevChatter.Bot.Core.Systems.Chat;
using DevChatter.Bot.Core.Systems.Streaming;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading;
using UnitTests.Fakes;
using Xunit;

namespace UnitTests.Core.BotModules.VotingModules
{
    public class VoteCommand_SetEndShould
    {
        [Fact]
        public void SetIsVoteActiveToFalse_GivenVoteEnded()
        {
            var (command, args, votingSystem, automationSystem) = GetTestOperation();

            command.TestIt(new Mock<IChatClient>().Object, args);

            votingSystem.IsVoteActive.Should().BeFalse();
        }

        [Fact]
        public void ScheduleEndOfVoting_GivenSpecifiedTimeLimit()
        {
            var (operation, args, _, automationSystem) = GetTestOperation();
            var chatClient = new Mock<IChatClient>();

            args.Arguments.Add("2s");

            operation.TestIt(chatClient.Object, args);
            IIntervalAction action = automationSystem.IntervalAction;

            action.Should().BeOfType<OneTimeCallBackAction>();

            action.IsTimeToRun().Should().BeFalse();

            Thread.Sleep(TimeSpan.FromSeconds(2));

            action.IsTimeToRun().Should().BeTrue();
            action.Invoke();
            chatClient.Verify(c => c.SendMessage(VotingMessages.NO_WINNER));
        }

        private static (TestableVoteCommand, CommandReceivedEventArgs, VotingSystem, FakeActionSystem) GetTestOperation(ChatUser chatUser = null)
        {
            var mock = new Mock<IVotingDisplayNotification>();
            var votingSystem = new VotingSystem(mock.Object);
            votingSystem.StartVote(new List<string> {"A", "B", "C"});
            var automationSystem = new FakeActionSystem();
            var endVoteOperation = new TestableVoteCommand(new Mock<IRepository>().Object,
                votingSystem, automationSystem);

            var commandReceivedEventArgs = new CommandReceivedEventArgs
            {
                Arguments = new List<string> {"end"},
                ChatUser = chatUser ?? new ChatUser { Role = UserRole.Mod }
            };
            return (endVoteOperation, commandReceivedEventArgs, votingSystem, automationSystem);
        }
    }
}
