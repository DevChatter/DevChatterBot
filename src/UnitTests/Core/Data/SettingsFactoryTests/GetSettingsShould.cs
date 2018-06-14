using System.Collections.Generic;
using DevChatter.Bot.Core.Data;
using DevChatter.Bot.Core.Data.Model;
using DevChatter.Bot.Core.Data.Specifications;
using DevChatter.Bot.Core.Games.Roulette;
using FluentAssertions;
using Moq;
using Xunit;

namespace UnitTests.Core.Data.SettingsFactoryTests
{
    public class GetSettingsShould
    {
        [Fact]
        public void ReturnSomething_GivenSomething()
        {
            var mock = new Mock<IRepository>();
            var settingsFactory = new SettingsFactory(mock.Object);

            var commandSettingsEntities = new List<CommandSettingsEntity>
            {
                new CommandSettingsEntity { Key = "ProtectSubscribers", Value = "false" },
                new CommandSettingsEntity { Key = "CoinsReward", Value = "23" },
                new CommandSettingsEntity { Key = "TimeoutDurationInSeconds", Value = "34" },
                new CommandSettingsEntity { Key = "WinPercentageChance", Value = "45" },
            };

            mock.Setup(x => x.List(It.IsAny<CommandSettingsPolicy>())).Returns(commandSettingsEntities);

            var rouletteSettings = settingsFactory.GetSettings<RouletteSettings>();

            rouletteSettings.Should().NotBeNull();
            rouletteSettings.ProtectSubscribers.Should().BeFalse();
            rouletteSettings.CoinsReward.Should().Be(23);
            rouletteSettings.TimeoutDurationInSeconds.Should().Be(34);
            rouletteSettings.WinPercentageChance.Should().Be(45);
        }
    }
}
