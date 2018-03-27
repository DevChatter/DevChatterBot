using System;
using DevChatter.Bot.Core.Events;

namespace DevChatter.Bot.Core.Automation
{
    public class CurrencyUpdate : IIntervalAction
    {
        private readonly int _intervalInMinutes;
        private readonly CurrencyGenerator _currencyGenerator;
        private DateTime _nextRunTime;

        public CurrencyUpdate(int intervalInMinutes, CurrencyGenerator currencyGenerator)
        {
            _intervalInMinutes = intervalInMinutes;
            _currencyGenerator = currencyGenerator;
            SetNextRunTime();
        }

        public bool IsTimeToRun() => DateTime.Now > _nextRunTime;

        public void Invoke()
        {
            _currencyGenerator.UpdateCurrency();
            SetNextRunTime();
        }

        private void SetNextRunTime()
        {
            _nextRunTime = DateTime.Now.AddMinutes(_intervalInMinutes);
        }
    }
}