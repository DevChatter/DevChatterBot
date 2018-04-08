using System.Collections.Generic;
using System.Linq;
using DevChatter.Bot.Core.Data;
using DevChatter.Bot.Core.Data.Model;
using DevChatter.Bot.Core.Data.Specifications;

namespace UnitTests.Fakes
{
    public class FakeRepo : IRepository
    {
        public object SingleToReturn { get; set; }
        public T Single<T>(ISpecification<T> spec) where T : DataEntity
        {
            return SingleToReturn as T;
        }

	    public List<object> ListToReturn { get; set; }
        public List<T> List<T>(ISpecification<T> spec) where T : DataEntity
        {
	        return (ListToReturn ?? Enumerable.Empty<object>()).OfType<T>().ToList();
        }

        public T Create<T>(T dataItem) where T : DataEntity
        {
            throw new System.NotImplementedException();
        }

        public T Update<T>(T dataItem) where T : DataEntity
        {
            throw new System.NotImplementedException();
        }

        public void Update<T>(List<T> dataItemList) where T : DataEntity
        {
        }

        public void Create<T>(List<T> dataItemList) where T : DataEntity
        {
        }

        public void Remove<T>(T dataItem) where T : DataEntity
        {
        }
    }
}