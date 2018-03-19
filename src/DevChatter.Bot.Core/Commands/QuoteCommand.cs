using System;
using System.Collections.Generic;
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
        private readonly Random _random = new Random();

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
                    HandleQuoteRequest(triggeringClient, requestQuoteId);
                }
                else if (argumentOne == "add")
                {
                    HandleQuoteAddition();
                }
                else
                {
                    // TODO: Handle this eroneous case
                }
            }
            else
            {
                HandleRandomQuoteRequest(triggeringClient);
            }
        }

        private void HandleRandomQuoteRequest(IChatClient triggeringClient)
        {
            List<QuoteEntity> quoteEntities = _repository.List(QuoteEntityPolicy.All);
            int selectedIndex = _random.Next(quoteEntities.Count);
            triggeringClient.SendMessage(quoteEntities[selectedIndex].ToString());
        }

        private void HandleQuoteAddition()
        {
            // TODO Check if it's an add
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