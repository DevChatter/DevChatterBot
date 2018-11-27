using DevChatter.Bot.Core.Events.Args;
using DevChatter.Bot.Modules.WastefulGame.Data;
using DevChatter.Bot.Modules.WastefulGame.Model;
using DevChatter.Bot.Modules.WastefulGame.Model.Specifications;
using System.Collections.Generic;
using System.Linq;

namespace DevChatter.Bot.Modules.WastefulGame.Commands.Operations
{
    public class BuyShopItemOperation :BaseGameCommandOperation
    {
        private readonly IGameRepository _gameRepository;

        public BuyShopItemOperation(IGameRepository gameRepository)
        {
            _gameRepository = gameRepository;
        }

        public override List<string> OperandWords { get; }
            = new List<string> { "Buy" };
        public override string TryToExecute(CommandReceivedEventArgs eventArgs,
            Survivor survivor)
        {
            string itemRequested = eventArgs.Arguments.ElementAtOrDefault(1);
            if (int.TryParse(itemRequested, out int itemId) && itemId > 0)
            {
                ShopItem shopItem =
                    _gameRepository.Single(ShopItemPolicy.ById(itemId));
                if (shopItem != null)
                {
                    if (survivor.BuyItem(shopItem))
                    {
                        _gameRepository.Update(survivor);
                        return $"{survivor.DisplayName} bought the {shopItem.Name} item.";
                    }
                    return $"{survivor.Money} is not enough money to buy the {shopItem.Name} item.";
                }
            }

            return "Please buy one of the listed items by Id.";
        }
    }
}
