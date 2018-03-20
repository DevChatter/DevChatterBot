using System.Linq;
using DevChatter.Bot.Core.ChatSystems;
using DevChatter.Bot.Core.Data;
using DevChatter.Bot.Core.Events;
using DevChatter.Bot.Core.Model;

namespace DevChatter.Bot.Core.Commands
{
    public class AddQuoteCommand : SimpleCommand
    {
        private readonly IRepository _repository;
        public AddQuoteCommand(IRepository repository)
        {
            _repository = repository;
            CommandText = "quoteadd";
            RoleRequired = UserRole.Mod;
        }

        public override void Process(IChatClient triggeringClient, CommandReceivedEventArgs eventArgs)
        {
            string quoteText = eventArgs?.Arguments?.ElementAtOrDefault(0);
            string quoteAuthor = eventArgs?.Arguments?.ElementAtOrDefault(1);

            // HACK: Replace with ValueGeneratedOnAdd in EF Core after removing in-memory database
            int count = _repository.List(QuoteEntityPolicy.All).Count;

            var quoteEntity = new QuoteEntity
            {
                AddedBy = eventArgs?.ChatUser?.DisplayName,
                Text = quoteText,
                Author = quoteAuthor,
                QuoteId = count + 1
            };

            QuoteEntity updatedEntity = _repository.Create(quoteEntity);

            triggeringClient.SendMessage($"Created quote # {updatedEntity.QuoteId}.");
        }
    }
}