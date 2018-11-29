using System.Collections.Generic;
using DevChatter.Bot.Modules.WastefulGame.Model;
using FluentAssertions;
using Xunit;

namespace WastefulGame.UnitTests.Model.SurvivorTests
{
    public class SellItemShould
    {
        [Fact]
        public void ReturnFalse_GivenNoItems()
        {
            var survivor = new Survivor();

            bool result = survivor.SellItem(0);

            result.Should().BeFalse();
        }

        [Fact]
        public void ReturnFalse_WhenTryingToSellOutOfRange()
        {
            var survivor = new Survivor
            {
                InventoryItems = new List<InventoryItem> { new InventoryItem() }
            };

            bool result = survivor.SellItem(5);

            result.Should().BeFalse();
        }

        [Fact]
        public void ReturnTrue_GivenSellingOnlyItem()
        {
            var survivor = new Survivor
            {
                InventoryItems = new List<InventoryItem> {new InventoryItem()}
            };

            bool result = survivor.SellItem(0);

            result.Should().BeTrue();
        }

        [Fact]
        public void RemoveCorrectItem_WhenSellingOneOfManyItems()
        {
            var itemToSell = new InventoryItem();
            var survivor = new Survivor
            {
                InventoryItems = new List<InventoryItem>
                {
                    new InventoryItem(),
                    new InventoryItem(),
                    itemToSell,
                }
            };

            bool result = survivor.SellItem(2);

            survivor.InventoryItems.Count.Should().Be(2);
            survivor.InventoryItems.Should().NotContain(itemToSell);
        }

        [Fact]
        public void RemoveLastItem_WhenSellingOnlyItem()
        {
            var survivor = new Survivor
            {
                InventoryItems = new List<InventoryItem> {new InventoryItem()}
            };

            bool result = survivor.SellItem(0);

            survivor.InventoryItems.Should().BeEmpty();
        }

        [Fact]
        public void IncreaseMoney_AfterSellingItem()
        {
            var survivor = new Survivor
            {
                Money = 100,
                InventoryItems = new List<InventoryItem> {new InventoryItem {}}
            };

            bool result = survivor.SellItem(0);

            survivor.Money.Should().Be(125);
        }
    }
}
