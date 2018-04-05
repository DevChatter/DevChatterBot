using DevChatter.Bot.Core.Commands;
using DevChatter.Bot.Core.Data.Model;
using DevChatter.Bot.Core.Events;
using DevChatter.Bot.Core.Systems.Chat;

namespace UnitTests.Fakes
{
    public class FakeCommand : IBotCommand
    {
        public FakeCommand(string commandText, bool isEnabled)
        {
            CommandText = commandText;
            IsEnabled = isEnabled;
        }

        public UserRole RoleRequired { get; } = UserRole.Everyone;
        public string CommandText { get; }
        public string HelpText { get; }
        public void Process(IChatClient chatClient, CommandReceivedEventArgs eventArgs)
        {
            ProcessWasCalled = true;
        }

        public bool ProcessWasCalled { get; set; } = false;

        public bool IsEnabled { get; }
    }
}