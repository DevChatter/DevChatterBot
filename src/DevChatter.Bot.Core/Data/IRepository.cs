using System.Collections.Generic;
using DevChatter.Bot.Core.Model;

namespace DevChatter.Bot.Core.Data
{
    public interface IRepository
    {
        T Single<T>(ISpecification<T> spec) where T : DataItem;
        List<T> List<T>(ISpecification<T> spec) where T : DataItem;
        T Create<T>(T dataItem) where T : DataItem;
        T Update<T>(T dataItem) where T : DataItem;
        void Create<T>(List<T> dataItemList) where T : DataItem;
    }
}