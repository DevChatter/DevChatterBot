using System.Linq;
using DevChatter.Bot.Core.ChatSystems;
using DevChatter.Bot.Core.Data;
using DevChatter.Bot.Core.Events;
using DevChatter.Bot.Core.Model;

namespace DevChatter.Bot.Core.Commands
{
    public class QuoteCommand : SimpleCommand
    {
        private readonly IRepository _repository;

        public QuoteCommand(IRepository repository)
        {
            _repository = repository;
            CommandText = "quote";
            RoleRequired = UserRole.Everyone;
        }

        public override void Process(IChatClient triggeringClient, CommandReceivedEventArgs eventArgs)
        {
            string argumentOne = eventArgs?.Arguments?.FirstOrDefault();
            if (argumentOne != null)
            {
                if (int.TryParse(argumentOne, out int requestQuoteId))
                {
                    // TODO: Get and return request
                    HandleQuoteRequest(triggeringClient, requestQuoteId);
                }
                else if (argumentOne == "add")
                {
                    HandleQuoteAddition();
                    // TODO Check if it's an add
                }
                else
                {
                    // TODO: Handle this eroneous case
                }
            }
            else
            {
                HandleRandomQuoteRequest();
                // TODO: Add Random Selection
            }
        }

        private void HandleRandomQuoteRequest()
        {
        }

        private void HandleQuoteAddition()
        {
        }

        private void HandleQuoteRequest(IChatClient triggeringClient, int requestQuoteId)
        {
            QuoteEntity quote = _repository.Single(QuoteEntityPolicy.ByQuoteId(requestQuoteId));
            if (quote == null)
            {
                triggeringClient.SendMessage($"I'm sorry, but we don't have a quote {requestQuoteId}... Yet...");
            }
            else
            {
                triggeringClient.SendMessage($"\"{quote.Message}\" - {quote.Author}, {quote.DateAdded.ToShortDateString()}");
            }
        }
    }
}