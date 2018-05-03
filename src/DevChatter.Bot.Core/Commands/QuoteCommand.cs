using DevChatter.Bot.Core.Commands.Operations;
using DevChatter.Bot.Core.Data;
using DevChatter.Bot.Core.Data.Model;
using DevChatter.Bot.Core.Data.Specifications;
using DevChatter.Bot.Core.Events.Args;
using DevChatter.Bot.Core.Extensions;
using DevChatter.Bot.Core.Systems.Chat;
using DevChatter.Bot.Core.Util;
using System.Collections.Generic;
using System.Linq;

namespace DevChatter.Bot.Core.Commands
{
    public class QuoteCommand : BaseCommand
    {
        private List<ICommandOperation> _operations;

        public List<ICommandOperation> Operations => _operations ?? (_operations = new List<ICommandOperation>
        {
            new GenericDeleteOperation<QuoteEntity>(Repository, UserRole.Mod,
                e => QuoteEntityPolicy.ByQuoteId(e.Arguments?.ElementAtOrDefault(1).SafeToInt())),
            new AddQuoteOperation(Repository),
        });

        public QuoteCommand(IRepository repository)
            : base(repository, UserRole.Everyone)
        {
            HelpText = $"Use !{PrimaryCommandText} to get a random quote, use !{PrimaryCommandText} [number] to get"
                       + $" a specific quote, or a moderator may use !{PrimaryCommandText} add \"Quote here.\" <author> to add"
                       + $" a quote. For example, \"!{PrimaryCommandText} add \"Oh what a day!\" Brendoneus creates a new quote.";
        }

        protected override void HandleCommand(IChatClient chatClient, CommandReceivedEventArgs eventArgs)
        {
            string argumentOne = eventArgs?.Arguments?.ElementAtOrDefault(0);

            var operationToUse = Operations.SingleOrDefault(x => x.ShouldExecute(argumentOne));

            if (operationToUse != null)
            {
                string message = operationToUse.TryToExecute(eventArgs);
                chatClient.SendMessage(message);
                return;
            }

            switch (argumentOne)
            {
                case null:
                    HandleRandomQuoteRequest(chatClient);
                    break;
                default:
                    if (int.TryParse(argumentOne, out int requestQuoteId))
                    {
                        HandleQuoteRequest(chatClient, requestQuoteId);
                    }

                    break;
            }
        }

        private void HandleRandomQuoteRequest(IChatClient triggeringClient)
        {
            List<QuoteEntity> quoteEntities = Repository.List(QuoteEntityPolicy.All);
            int selectedIndex = MyRandom.RandomNumber(0, quoteEntities.Count);
            triggeringClient.SendMessage(quoteEntities[selectedIndex].ToString());
        }

        private void HandleQuoteRequest(IChatClient triggeringClient, int requestQuoteId)
        {
            QuoteEntity quote = Repository.Single(QuoteEntityPolicy.ByQuoteId(requestQuoteId));
            triggeringClient.SendMessage(quote?.ToString() ?? $"I'm sorry, but we don't have a quote {requestQuoteId}... Yet...");
        }
    }
}
