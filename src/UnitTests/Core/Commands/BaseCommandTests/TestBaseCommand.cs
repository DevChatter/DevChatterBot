using DevChatter.Bot.Core.Commands;
using DevChatter.Bot.Core.Data;
using DevChatter.Bot.Core.Data.Model;
using DevChatter.Bot.Core.Events.Args;
using DevChatter.Bot.Core.Systems.Chat;

namespace UnitTests.Core.Commands.BaseCommandTests
{
    public class TestBaseCommand : BaseCommand
    {
        public TestBaseCommand(IRepository repository)
            : base(repository, UserRole.Everyone)
        {
        }

        protected override void HandleCommand(IChatClient chatClient, CommandReceivedEventArgs eventArgs)
        {
            throw new System.NotImplementedException();
        }
    }
}
