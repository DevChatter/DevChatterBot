using DevChatter.Bot.Core.Data;
using DevChatter.Bot.Core.Data.Model;
using DevChatter.Bot.Core.Data.Specifications;
using DevChatter.Bot.Core.Events.Args;
using System.Collections.Generic;
using System.Linq;

namespace DevChatter.Bot.Core.Commands.Operations
{
    public class AddQuoteOperation : BaseCommandOperation
    {
        private IRepository _repository;
        public AddQuoteOperation(IRepository repository)
        {
            _repository = repository;
        }

        public override List<string> OperandWords => new List<string> { "add", "new", "create" };

        public override string HelpText => "";

        public override string TryToExecute(CommandReceivedEventArgs eventArgs)
        {
            string quoteText = eventArgs?.Arguments?.ElementAtOrDefault(1);
            string author = eventArgs?.Arguments?.ElementAtOrDefault(2);

            if (!eventArgs.ChatUser.CanUserRunCommand(UserRole.Mod))
            {
                return $"Please ask a moderator to add this quote, {eventArgs.ChatUser.DisplayName}.";
            }

            int count = _repository.List(QuoteEntityPolicy.All).Count;

            var quoteEntity = new QuoteEntity
            {
                AddedBy = eventArgs.ChatUser.DisplayName,
                Text = quoteText,
                Author = author,
                QuoteId = count + 1
            };

            QuoteEntity updatedEntity = _repository.Create(quoteEntity);

            return $"Created quote # {updatedEntity.QuoteId}.";
        }
    }
}
