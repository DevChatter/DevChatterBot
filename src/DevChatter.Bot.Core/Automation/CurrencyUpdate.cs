using DevChatter.Bot.Core.Data;
using DevChatter.Bot.Core.Events;
using DevChatter.Bot.Core.Settings;
using DevChatter.Bot.Core.Util;
using System;
using System.Linq.Expressions;

namespace DevChatter.Bot.Core.Automation
{
    public class CurrencyUpdate
        : IIntervalAction, IAutomatedItem, IInterval, IAutomatedAction
    {
        private readonly ICurrencyGenerator _currencyGenerator;
        private readonly IClock _clock;
        private DateTime _nextRunTime;

        private readonly CurrencySettings _currencySettings;
        public int IntervalInMinutes => _currencySettings?.IntervalInMinutes ?? 1;
        public TimeSpan IntervalTimeSpan { get; }
        public Expression<Action> Action => () => _currencyGenerator.UpdateCurrency();

        public CurrencyUpdate(ICurrencyGenerator currencyGenerator, ISettingsFactory settingsFactory, IClock clock)
        {
            _currencyGenerator = currencyGenerator;
            _clock = clock;
            _currencySettings = settingsFactory.GetSettings<CurrencySettings>();
            IntervalTimeSpan = TimeSpan.FromMinutes(IntervalInMinutes);
            SetNextRunTime();
        }

        public bool IsTimeToRun() => _clock.Now >= _nextRunTime;

        public void Invoke()
        {
            _currencyGenerator.UpdateCurrency();
            SetNextRunTime();
        }

        public bool IsDone => false;

        private void SetNextRunTime()
        {
            _nextRunTime = _clock.Now.AddMinutes(_currencySettings.IntervalInMinutes);
        }

    }
}
