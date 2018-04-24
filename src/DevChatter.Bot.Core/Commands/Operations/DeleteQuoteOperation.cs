using DevChatter.Bot.Core.Data;
using DevChatter.Bot.Core.Data.Model;
using DevChatter.Bot.Core.Data.Specifications;
using DevChatter.Bot.Core.Events.Args;
using System.Collections.Generic;
using System.Linq;

namespace DevChatter.Bot.Core.Commands.Operations
{
    public class DeleteQuoteOperation : BaseCommandOperation
    {
        private IRepository _repository;
        public DeleteQuoteOperation(IRepository repository)
        {
            _repository = repository;
        }

        public override List<string> OperandWords => new List<string> { "rm", "del", "remove", "delete" };

        public override string HelpText => "";

        public override string TryToExecute(CommandReceivedEventArgs eventArgs)
        {
            if (!eventArgs.ChatUser.CanUserRunCommand(UserRole.Mod))
            {
                return "You need to be a moderator to delete a quote.";
            }

            string quoteNumberString = eventArgs?.Arguments?.ElementAtOrDefault(1);

            if (int.TryParse(quoteNumberString, out int quoteNumber) && quoteNumber > 0)
            {
                var quote = _repository.Single(QuoteEntityPolicy.ByQuoteId(quoteNumber));

                if (quote == null)
                {
                    return $"We don't seem to have a quote {quoteNumber}.";
                }

                _repository.Remove(quote);
                return $"Deleted quote \"{quote.Text}\" - {quoteNumber}";
            }
            else
            {
                return "Please specify a valid quote number to delete.";
            }
        }
    }
}
