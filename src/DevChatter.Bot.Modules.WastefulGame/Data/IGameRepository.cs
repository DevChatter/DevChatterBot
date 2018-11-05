using System.Collections.Generic;
using DevChatter.Bot.Core.Data.Model;
using DevChatter.Bot.Core.Data.Specifications;

namespace DevChatter.Bot.Modules.WastefulGame.Data
{
    public interface IGameRepository
    {
        T Single<T>(ISpecification<T> spec) where T : class;
        List<T> List<T>(ISpecification<T> spec = null) where T : class;
        T Create<T>(T dataItem) where T : class;
        T Update<T>(T dataItem) where T : class;
        void Remove<T>(T dataItem) where T : class;
        void Remove<T>(List<T> dataItems) where T : class;
        void Update<T>(List<T> dataItemList) where T : class;
        void Create<T>(List<T> dataItemList) where T : class;
    }
}
