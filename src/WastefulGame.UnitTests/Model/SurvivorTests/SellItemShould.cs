using System.Collections.Generic;
using DevChatter.Bot.Modules.WastefulGame.Model;
using FluentAssertions;
using Xunit;

namespace WastefulGame.UnitTests.Model.SurvivorTests
{
    public class SellItemShould
    {
        [Fact]
        public void ReturnNull_GivenNoItems()
        {
            var survivor = new Survivor();

            InventoryItem result = survivor.SellItem(0);

            result.Should().BeNull();
        }

        [Fact]
        public void ReturnNull_WhenTryingToSellOutOfRange()
        {
            var survivor = new Survivor
            {
                InventoryItems = new List<InventoryItem> { new InventoryItem() }
            };

            InventoryItem result = survivor.SellItem(5);

            result.Should().BeNull();
        }

        [Fact]
        public void ReturnItem_GivenSellingOnlyItem()
        {
            var item = new InventoryItem();
            var survivor = new Survivor
            {
                InventoryItems = new List<InventoryItem> {item}
            };

            InventoryItem result = survivor.SellItem(0);

            result.Should().Be(item);
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

            survivor.SellItem(2);

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

            survivor.SellItem(0);

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

            survivor.SellItem(0);

            survivor.Money.Should().Be(125);
        }
    }
}
