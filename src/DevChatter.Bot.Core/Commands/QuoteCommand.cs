using System.Collections.Generic;
using System.Linq;
using DevChatter.Bot.Core.Data;
using DevChatter.Bot.Core.Data.Model;
using DevChatter.Bot.Core.Data.Specifications;
using DevChatter.Bot.Core.Events.Args;
using DevChatter.Bot.Core.Systems.Chat;
using DevChatter.Bot.Core.Util;

namespace DevChatter.Bot.Core.Commands
{
    public class QuoteCommand : BaseCommand
    {
        private readonly IRepository _repository;

        public QuoteCommand(IRepository repository)
            : base(repository, UserRole.Everyone)
        {
            _repository = repository;
            HelpText = $"Use !{PrimaryCommandText} to get a random quote, use !{PrimaryCommandText} [number] to get"
                       + $" a specific quote, or a moderator may use !{PrimaryCommandText} add \"Quote here.\" <author> to add"
                       + $" a quote. For example, \"!{PrimaryCommandText} add \"Oh what a day!\" Brendoneus creates a new quote.";
        }

        public override void Process(IChatClient chatClient, CommandReceivedEventArgs eventArgs)
        {
            string argumentOne = eventArgs?.Arguments?.ElementAtOrDefault(0);
            string quoteText = eventArgs?.Arguments?.ElementAtOrDefault(1);
            string author = eventArgs?.Arguments?.ElementAtOrDefault(2);

            switch (argumentOne)
            {
                case null:
                    HandleRandomQuoteRequest(chatClient);
                    break;
                case "add":
                    AddNewQuote(chatClient, quoteText, author, eventArgs.ChatUser);
                    break;
                default:
                    if (int.TryParse(argumentOne, out int requestQuoteId))
                    {
                        HandleQuoteRequest(chatClient, requestQuoteId);
                    }

                    break;
            }
        }

        private void AddNewQuote(IChatClient chatClient, string quoteText, string author, ChatUser chatUser)
        {
            if (!chatUser.CanUserRunCommand(UserRole.Mod))
            {
                chatClient.SendMessage($"Please ask a moderator to add this quote, {chatUser.DisplayName}.");
                return;
            }

            int count = _repository.List(QuoteEntityPolicy.All).Count;

            var quoteEntity = new QuoteEntity
            {
                AddedBy = chatUser.DisplayName,
                Text = quoteText,
                Author = author,
                QuoteId = count + 1
            };

            QuoteEntity updatedEntity = _repository.Create(quoteEntity);

            chatClient.SendMessage($"Created quote # {updatedEntity.QuoteId}.");
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
