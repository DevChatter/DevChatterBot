using System.Collections.Generic;

namespace DevChatter.Bot.Core.Events
{
    public interface ICurrencyGenerator
    {
        void AddCurrencyTo(List<string> listOfNames, int tokensToAdd);
        void AddCurrencyTo(string displayName, int tokensToAdd);
        bool RemoveCurrencyFrom(string userName, int tokensToRemove);
        void UpdateCurrency();
    }
}
