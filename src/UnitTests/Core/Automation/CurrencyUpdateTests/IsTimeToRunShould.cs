using DevChatter.Bot.Core;
using DevChatter.Bot.Core.Automation;
using DevChatter.Bot.Core.Data;
using DevChatter.Bot.Core.Data.Model;
using DevChatter.Bot.Core.Data.Specifications;
using DevChatter.Bot.Core.Events;
using DevChatter.Bot.Core.Systems.Chat;
using Moq;
using System.Collections.Generic;
using UnitTests.Fakes;
using Xunit;

namespace UnitTests.Core.Automation.CurrencyUpdateTests
{
    public class IsTimeToRunShould
    {
        [Fact]
        public void ReturnFalse_UponInitialCreation()
        {
            var currencyUpdate = new CurrencyUpdate(null, new Mock<ISettingsFactory>().Object, new FakeClock());

            bool result = currencyUpdate.IsTimeToRun();

            Assert.False(result);
        }

        [Fact]
        public void ReturnTrue_AfterWaitingFullInterval()
        {
            const int intervalInMinutes = 1;
            var fakeClock = new FakeClock();
            var currencyUpdate = new CurrencyUpdate(null, new Mock<ISettingsFactory>().Object, fakeClock);
            fakeClock.Now = fakeClock.Now.AddMinutes(intervalInMinutes);

            Assert.True(currencyUpdate.IsTimeToRun());
        }

        [Fact]
        public void ReturnFalse_AfterInvokingAction()
        {
            const int intervalInMinutes = 1;
            var repository = new Mock<IRepository>();
            repository.Setup(x => x.List(It.IsAny<ISpecification<ChatUser>>())).Returns(new List<ChatUser>());
            var settingsFactory = new Mock<ISettingsFactory>();
            var currencyGenerator = new CurrencyGenerator(new List<IChatClient>(), new ChatUserCollection(repository.Object), settingsFactory.Object);
            var fakeClock = new FakeClock();
            var currencyUpdate = new CurrencyUpdate(currencyGenerator, settingsFactory.Object, fakeClock);

            fakeClock.Now = fakeClock.Now.AddMinutes(intervalInMinutes);
            Assert.True(currencyUpdate.IsTimeToRun());

            fakeClock.Now = fakeClock.Now.AddMinutes(intervalInMinutes);
            currencyUpdate.Invoke();
            Assert.False(currencyUpdate.IsTimeToRun());
        }
    }
}
