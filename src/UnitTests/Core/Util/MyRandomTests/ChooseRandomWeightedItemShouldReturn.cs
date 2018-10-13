using System.Collections.Generic;
using DevChatter.Bot.Core.Data.Model;
using DevChatter.Bot.Core.Util;
using FluentAssertions;
using Xunit;

namespace UnitTests.Core.Util.MyRandomTests
{
    public class ChooseRandomWeightedItemShouldReturn
    {
        [Fact]
        public void Null_GivenEmptyList()
        {
            var weightedItems = new List<IntervalMessage>();

            var weightedItem = MyRandom.ChooseRandomWeightedItem(weightedItems);

            weightedItem.Should().BeNull();
        }

        [Fact]
        public void Null_GivenNoWeightedItem()
        {
            var item = new IntervalMessage();

            var weightedItem = MyRandom.ChooseRandomWeightedItem(new[]{ item });

            weightedItem.Should().BeNull();
        }

        [Fact]
        public void SingleItem_GivenOneWeightedItem()
        {
            var item = new IntervalMessage(1,"foo", 354);

            var weightedItem = MyRandom.ChooseRandomWeightedItem(new[]{ item });

            weightedItem.Should().Be(item);
        }

        [Fact]
        public void SingleItem_GivenMultipleItemsOnlyOneWeighted()
        {
            var item = new IntervalMessage(0, "foo", 2);
            var weightedItems = new List<IntervalMessage>
            {
                item,
                new IntervalMessage(0, "bar", 0),
                new IntervalMessage(0, "fiz", 0),
            };

            var weightedItem = MyRandom.ChooseRandomWeightedItem(weightedItems);

            weightedItem.Should().Be(item);
        }

        [Fact]
        public void AnItemFromCollection_GivenMultiples()
        {
            var weightedItems = new List<IntervalMessage>
            {
                new IntervalMessage(10, "foo", 3),
                new IntervalMessage(12, "bar", 3),
                new IntervalMessage(13, "fiz", 3),
            };

            var chosenItem = MyRandom.ChooseRandomWeightedItem(weightedItems);

            weightedItems.Should().Contain(chosenItem);
        }
    }
}
