using System;
using DevChatter.Bot.Core.Events;

namespace DevChatter.Bot.Core.Automation
{
    public class CurrencyUpdate : IIntervalAction
    {
        private readonly int _intervalInMinutes;
        private readonly CurrencyGenerator _currencyGenerator;
        private DateTime _previousRunTime = DateTime.Now;

        public CurrencyUpdate(int intervalInMinutes, CurrencyGenerator currencyGenerator)
        {
            _intervalInMinutes = intervalInMinutes;
            _currencyGenerator = currencyGenerator;
        }

        public bool IsTimeToRun()
        {
            if (DateTime.Now > _previousRunTime.AddMinutes(_intervalInMinutes))
            {
                return true;
            }
            return false;
        }

        public void Invoke()
        {
            _currencyGenerator.UpdateCurrency();
            _previousRunTime = DateTime.Now;
        }
    }
}