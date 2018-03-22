using System;
using System.Collections.Generic;
using DevChatter.Bot.Core.Automation;
using DevChatter.Bot.Core.ChatSystems;
using DevChatter.Bot.Core.Events;
using Pose;
using UnitTests.Fakes;
using Xunit;

namespace UnitTests.Core.Automation.CurrencyUpdateTests
{
    public class IsTimeToRunShould
    {
        [Fact]
        public void ReturnFalse_UponInitialCreation()
        {
            var currencyUpdate = new CurrencyUpdate(1, null);

            bool result = currencyUpdate.IsTimeToRun();

            Assert.False(result);
        }

        [Fact]
        public void ReturnTrue_AfterWaitingFullInterval()
        {
            const int intervalInMinutes = 1;
            Shim shim = Shim.Replace(() => DateTime.Now).With(() => DateTime.Now.AddMinutes(intervalInMinutes));
            var currencyUpdate = new CurrencyUpdate(intervalInMinutes, null);

            PoseContext.Isolate(() => Assert.True(currencyUpdate.IsTimeToRun()), shim);
        }

        [Fact]
        public void ReturnFalse_AfterInvokingAction()
        {
            // TODO: Add the shim in here, because the test would pass regardless
            const int intervalInMinutes = 1;
            var currencyUpdate = new CurrencyUpdate(intervalInMinutes, new CurrencyGenerator(new List<IChatClient>(), new FakeRepo()));

            bool result = false;
            currencyUpdate.Invoke();
            result = currencyUpdate.IsTimeToRun();
            Assert.False(result);
        }
    }
}