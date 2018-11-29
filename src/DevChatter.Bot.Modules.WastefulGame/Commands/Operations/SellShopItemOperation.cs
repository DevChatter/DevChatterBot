using DevChatter.Bot.Core.Events.Args;
using DevChatter.Bot.Modules.WastefulGame.Data;
using DevChatter.Bot.Modules.WastefulGame.Model;
using System.Collections.Generic;
using System.Linq;

namespace DevChatter.Bot.Modules.WastefulGame.Commands.Operations
{
    public class SellShopItemOperation : BaseGameCommandOperation
    {
        public const string BAD_ARGUMENT_MESSAGE
            = "Please specify the item you wish to sell using its inventory letter.";

        public const string OUT_OF_RANGE_MESSAGE = "Which item did you want to sell?";
        public const string SOLD_FORMAT_STRING = "Sold {0} for 25 coins.";

        private readonly IGameRepository _repository;

        public SellShopItemOperation(IGameRepository repository)
        {
            _repository = repository;
        }

        public override List<string> OperandWords { get; }
            = new List<string> { "sell" };
        public override string TryToExecute(CommandReceivedEventArgs eventArgs,
            Survivor survivor)
        {
            string itemKey = eventArgs.Arguments.ElementAtOrDefault(1);
            if (itemKey == null || itemKey.Length != 1 || !char.IsLetter(itemKey,0))
            {
                return BAD_ARGUMENT_MESSAGE;
            }

            int itemIndex = itemKey.ToUpper().Single() - 'A';
            if (itemIndex < 0 || itemIndex > 4)
            {
                return OUT_OF_RANGE_MESSAGE;
            }

            InventoryItem soldItem = survivor.SellItem(itemIndex);
            if (soldItem != null)
            {
                _repository.Update(survivor);
                return string.Format(SOLD_FORMAT_STRING, soldItem.Name);
            }

            return OUT_OF_RANGE_MESSAGE;
        }
    }
}
