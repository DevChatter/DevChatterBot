using System.Linq;
using DevChatter.Bot.Modules.WastefulGame.Model;
using FluentAssertions;
using Xunit;

namespace WastefulGame.UnitTests.Model.SurvivorTests
{
    public class BuyItemShould
    {
        [Fact]
        public void ReturnFalse_GivenNotEnoughMoney()
        {
            var survivor = new Survivor { Money = 10 };
            var shopItem = new ShopItem { Price = 11 };

            bool result = survivor.BuyItem(shopItem);

            result.Should().BeFalse();
        }

        [Fact]
        public void ReturnTrue_GivenEnoughMoney()
        {
            var survivor = new Survivor { Money = 10 };
            var shopItem = new ShopItem { Price = 10 };

            bool result = survivor.BuyItem(shopItem);

            result.Should().BeTrue();
        }

        [Fact]
        public void ReduceMoneyByPrice_WhenSuccessful()
        {
            var survivor = new Survivor { Money = 100 };
            var shopItem = new ShopItem { Price = 10 };

            survivor.BuyItem(shopItem);

            survivor.Money.Should().Be(90);
        }

        [Fact]
        public void AddItemToInventory_WhenSuccessful()
        {
            const string itemName = "Axe";
            var survivor = new Survivor { Money = 10 };
            var shopItem = new ShopItem { Price = 10, Name = itemName };

            survivor.BuyItem(shopItem);

            survivor.InventoryItems.Single().Name.Should().Be(itemName);
        }
    }
}
