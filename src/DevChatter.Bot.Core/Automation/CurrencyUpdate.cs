using System;
using DevChatter.Bot.Core.Events;
using DevChatter.Bot.Core.Util;

namespace DevChatter.Bot.Core.Automation
{
    public class CurrencyUpdate : IIntervalAction
    {
        public readonly int IntervalInMinutes;
        private readonly ICurrencyGenerator _currencyGenerator;
        private readonly IClock _clock;
        private DateTime _nextRunTime;

        public CurrencyUpdate(int intervalInMinutes, ICurrencyGenerator currencyGenerator, IClock clock)
        {
            IntervalInMinutes = intervalInMinutes;
            _currencyGenerator = currencyGenerator;
            _clock = clock;
            SetNextRunTime();
        }

        public string Name { get; } = nameof(CurrencyUpdate);
        public bool IsTimeToRun() => _clock.Now >= _nextRunTime;

        public void Invoke()
        {
            _currencyGenerator.UpdateCurrency();
            SetNextRunTime();
        }

        private void SetNextRunTime()
        {
            _nextRunTime = _clock.Now.AddMinutes(IntervalInMinutes);
        }
    }
}
