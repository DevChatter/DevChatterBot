using DevChatter.Bot.Core.Events.Args;
using DevChatter.Bot.Modules.WastefulGame.Commands.Operations;
using DevChatter.Bot.Modules.WastefulGame.Data;
using DevChatter.Bot.Modules.WastefulGame.Model;
using FluentAssertions;
using Moq;
using System.Linq;
using Xunit;

namespace WastefulGame.UnitTests.Commands.Operations
{
    public class SellShopItemOperationShould
    {
        [Fact]
        public void AskForArgument_GivenTooFewArguments()
        {
            var (operation, eventArgs, survivor, _) = SetUpTest(0, "sell");

            string result = operation.TryToExecute(eventArgs, survivor);

            result.Should().Be(SellShopItemOperation.BAD_ARGUMENT_MESSAGE);
        }

        [Fact]
        public void AskForArgument_GivenInvalidArgument()
        {
            var (operation, eventArgs, survivor, _) = SetUpTest(0, "sell", "1");

            string result = operation.TryToExecute(eventArgs, survivor);

            result.Should().Be(SellShopItemOperation.BAD_ARGUMENT_MESSAGE);
        }

        [Fact]
        public void ShowError_GivenOutOfRangeArgument()
        {
            var (operation, eventArgs, survivor, _) = SetUpTest(0, "sell", "N");

            string result = operation.TryToExecute(eventArgs, survivor);

            result.Should().Be(SellShopItemOperation.OUT_OF_RANGE_MESSAGE);
        }

        [Fact]
        public void ShowError_GivenNoItems()
        {
            var (operation, eventArgs, survivor, _) = SetUpTest(0, "sell", "A");

            string result = operation.TryToExecute(eventArgs, survivor);

            result.Should().Be(SellShopItemOperation.OUT_OF_RANGE_MESSAGE);
        }

        [Fact]
        public void ShowSuccess_GivenItemSold()
        {
            var (operation, eventArgs, survivor, _) = SetUpTest(1, "sell", "A");
            string expected = string.Format(SellShopItemOperation.SOLD_FORMAT_STRING, survivor.InventoryItems[0].Name);

            string result = operation.TryToExecute(eventArgs, survivor);

            result.Should().Be(expected);
        }

        [Fact]
        public void SaveData_GivenItemSold()
        {
            var (operation, eventArgs, survivor, repo) = SetUpTest(1, "sell", "A");

            string result = operation.TryToExecute(eventArgs, survivor);

            repo.Verify(x => x.Update(survivor));
        }

        private static (SellShopItemOperation, CommandReceivedEventArgs, Survivor, Mock<IGameRepository>)
            SetUpTest(int numberOfItems, params string[] arguments)
        {
            var repo = new Mock<IGameRepository>();
            var operation = new SellShopItemOperation(repo.Object);
            var eventArgs = new CommandReceivedEventArgs
            {
                Arguments = arguments
            };

            var survivor = new Survivor();
            for (int i = 0; i < numberOfItems; i++)
            {
                survivor.InventoryItems.Add(new InventoryItem{Name = i.ToString()});
            }

            return (operation, eventArgs, survivor, repo);
        }

    }
}
