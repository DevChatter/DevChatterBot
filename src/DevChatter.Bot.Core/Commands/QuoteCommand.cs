using System.Collections.Generic;
using System.Linq;
using DevChatter.Bot.Core.Data;
using DevChatter.Bot.Core.Data.Model;
using DevChatter.Bot.Core.Data.Specifications;
using DevChatter.Bot.Core.Events;
using DevChatter.Bot.Core.Systems.Chat;
using DevChatter.Bot.Core.Util;

namespace DevChatter.Bot.Core.Commands
{
    public class QuoteCommand : BaseCommand
    {
        private readonly IRepository _repository;

        public QuoteCommand(IRepository repository)
            : base("quote", UserRole.Everyone)
        {
            _repository = repository;
        }

        public override void Process(IChatClient chatClient, CommandReceivedEventArgs eventArgs)
        {
            string argumentOne = eventArgs?.Arguments?.FirstOrDefault();
            if (argumentOne != null)
            {
                if (int.TryParse(argumentOne, out int requestQuoteId))
                {
                    HandleQuoteRequest(chatClient, requestQuoteId);
                }
                // TODO: Handle this eroneous case
            }
            else
            {
                HandleRandomQuoteRequest(chatClient);
            }
        }

        private void HandleRandomQuoteRequest(IChatClient triggeringClient)
        {
            List<QuoteEntity> quoteEntities = _repository.List(QuoteEntityPolicy.All);
            int selectedIndex = MyRandom.RandomNumber(0, quoteEntities.Count);
            triggeringClient.SendMessage(quoteEntities[selectedIndex].ToString());
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
                triggeringClient.SendMessage(quote.ToString());
            }
        }
    }
}