using System.Linq;
using System.Net.Http.Headers;
using DevChatter.Bot.Core.Data;
using DevChatter.Bot.Core.Data.Model;
using DevChatter.Bot.Core.Data.Specifications;
using DevChatter.Bot.Core.Events;
using DevChatter.Bot.Core.Systems.Chat;

namespace DevChatter.Bot.Core.Commands
{
	public class AddQuoteCommand : BaseCommand
    {
        private readonly IRepository _repository;
        public AddQuoteCommand(IRepository repository)
            : base(repository, UserRole.Mod)
        {
            _repository = repository;
            HelpText = "Example usage !quoteadd \"this is the quote\" @Brendoneus";
        }

        public override void Process(IChatClient chatClient, CommandReceivedEventArgs eventArgs)
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

            chatClient.SendMessage($"Created quote # {updatedEntity.QuoteId}.");
        }
    }
}