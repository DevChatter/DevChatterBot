using System.Linq;
using DevChatter.Bot.Core.Commands;
using DevChatter.Bot.Core.Data;
using DevChatter.Bot.Core.Events.Args;
using DevChatter.Bot.Core.Systems.Chat;

namespace UnitTests.Fakes
{
    public class FakeCommand : BaseCommand
    {
        public FakeCommand(IRepository repository)
            : base(repository)
        {
            NotifyWordsModified();
        }

        protected override void HandleCommand(IChatClient chatClient, CommandReceivedEventArgs eventArgs)
        {
            ProcessWasCalled = true;
        }

        public bool ProcessWasCalled { get; set; }
        public string CommandText => CommandWords.First().Word;
    }
}
