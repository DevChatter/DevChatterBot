using DevChatter.Bot.Core.Data;
using DevChatter.Bot.Core.Events;
using DevChatter.Bot.Core.Games.Slots;
using FluentAssertions;
using Moq;
using System.Collections.Generic;
using Xunit;

namespace UnitTests.Core.Games.Slots.SlotsCommandTests
{
    public class CalculatePayoutShould
    {
        private readonly SlotsCommand _slotsCommand;

        private static readonly SlotEmote _noPayout1 = new SlotEmote("Kappa", 0, 0, 0);
        private static readonly SlotEmote _noPayout2 = new SlotEmote("KappaHD", 0, 0, 0);
        private static readonly SlotEmote _noPayout3 = new SlotEmote("KappaPride", 0, 0, 0);
        private static readonly SlotEmote _triplePayout = new SlotEmote("PogChamp", 100, 0, 0);
        private static readonly SlotEmote _doublePayout = new SlotEmote("SeriousSloth", 0, 50, 0);
        private static readonly SlotEmote _singlePayout = new SlotEmote("BrainSlug", 0, 0, 5);

        public CalculatePayoutShould()
        {
            _slotsCommand = new SlotsCommand(new Mock<IRepository>().Object,
                new Mock<ICurrencyGenerator>().Object,
                new Mock<ISettingsFactory>().Object);
        }

        [Fact]
        public void ReturnZero_GivenUnmatchedEmotes()
        {
            var emotes = new List<SlotEmote> { _noPayout1, _noPayout2, _noPayout3 };

            int payout = _slotsCommand.CalculatePayout(emotes);

            payout.Should().Be(0);
        }

        [Theory]
        [InlineData(0, 0, 0, 0)]
        [InlineData(1, 0, 0, 1)]
        [InlineData(1, 1, 0, 2)]
        [InlineData(1, 1, 1, 3)]
        [InlineData(10, 1, 1, 12)]
        public void ReturnSumOfPayouts_GivenSinglePayoutEmotes(int p1, int p2, int p3, int expected)
        {
            var emotes = new List<SlotEmote> { new SlotEmote("A", 1, 0, p1), new SlotEmote("B", 1, 0, p2), new SlotEmote("C", 1, 0, p3) };

            int payout = _slotsCommand.CalculatePayout(emotes);

            payout.Should().Be(expected);
        }

        [Fact]
        public void ReturnsTripleValueOnly_GivenMatchingEmotes()
        {
            var emotes = new List<SlotEmote> { _triplePayout, _triplePayout, _triplePayout };
            int payout = _slotsCommand.CalculatePayout(emotes);

            payout.Should().Be(_triplePayout.TriplePayout);
        }

        [Fact]
        public void ReturnsDoubleValue_GivenMatchingPair()
        {
            var emotes = new List<SlotEmote> { _doublePayout, _doublePayout, _noPayout1 };
            int payout = _slotsCommand.CalculatePayout(emotes);

            payout.Should().Be(_doublePayout.DoublePayout);
        }

        [Fact]
        public void ReturnsDoubleAndSingleValue_GivenMatchingPairWithSingle()
        {
            var emotes = new List<SlotEmote> { _doublePayout, _doublePayout, _singlePayout };
            int payout = _slotsCommand.CalculatePayout(emotes);

            payout.Should().Be(_doublePayout.DoublePayout + _singlePayout.SinglePayout);
        }
    }
}
