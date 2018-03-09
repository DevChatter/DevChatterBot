using System.Collections.Generic;
using DevChatter.Bot.Core.Data;

namespace DevChatter.Bot.Core
{
    public interface IRepository
    {
        List<T> List<T>(ISpecification<T> spec) where T : DataItem;
        T Create<T>(T dataItem) where T : DataItem;
        T Update<T>(T dataItem) where T : DataItem;
        List<T> Create<T>(List<T> dataItemList) where T : DataItem;
    }
}