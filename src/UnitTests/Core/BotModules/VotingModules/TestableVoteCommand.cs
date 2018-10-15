using DevChatter.Bot.Core.Automation;
using DevChatter.Bot.Core.BotModules.VotingModule;
using DevChatter.Bot.Core.Data;
using DevChatter.Bot.Core.Events.Args;
using DevChatter.Bot.Core.Systems.Chat;

namespace UnitTests.Core.BotModules.VotingModules
{
    internal class TestableVoteCommand : VoteCommand
    {
        public TestableVoteCommand(IRepository repository, VotingSystem votingSystem, IAutomatedActionSystem automatedActionSystem) : base(repository, votingSystem, automatedActionSystem)
        {
        }

        public void TestIt(IChatClient chatclient, CommandReceivedEventArgs eventArgs)
        {
            HandleCommand(chatclient, eventArgs);
        }
    }
}
