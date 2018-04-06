using DevChatter.Bot.Core.Commands;
using DevChatter.Bot.Core.Data.Model;
using DevChatter.Bot.Core.Events;
using DevChatter.Bot.Core.Systems.Chat;

namespace UnitTests.Fakes
{
    public class FakeCommand : BaseCommand
    {
        public FakeCommand(string commandText, bool isEnabled)
        {
            CommandText = commandText;
            IsEnabled = isEnabled;
            RoleRequired = UserRole.Everyone;
        }

        public override void Process(IChatClient chatClient, CommandReceivedEventArgs eventArgs)
        {
            ProcessWasCalled = true;
        }

        public bool ProcessWasCalled { get; set; } = false;
    }
}