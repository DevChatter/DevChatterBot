using System.Collections.Generic;

namespace DevChatter.Bot.Core.Events
{
    public interface ICurrencyGenerator
    {
        void AddCurrencyTo(IEnumerable<string> listOfNames, int tokensToAdd);
        void AddCurrencyTo(string displayName, int tokensToAdd);
        int RemoveCurrencyFrom(string userName, int tokensToRemove, bool takeAllIfInsufficient = false);
        void UpdateCurrency();
    }
}
