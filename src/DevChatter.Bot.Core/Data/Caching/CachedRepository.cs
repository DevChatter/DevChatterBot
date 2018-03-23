using System.Collections.Generic;
using DevChatter.Bot.Core.Model;

namespace DevChatter.Bot.Core.Data.Caching
{
    public class CachedRepository : IRepository
    {
        private readonly IRepository _repository;
        private readonly ICacheLayer _cacheLayer;

        public CachedRepository(IRepository repository, ICacheLayer cacheLayer)
        {
            _repository = repository;
            _cacheLayer = cacheLayer;
        }

        public T Single<T>(ISpecification<T> spec) where T : DataEntity
        {
            T item = _cacheLayer.TryGet<T>(spec.CacheKey);
            if (item == null)
            {
                item = _repository.Single(spec);
                _cacheLayer.Insert<T>(item, spec.CacheKey);
            }
            return item;
        }

        public List<T> List<T>(ISpecification<T> spec) where T : DataEntity
        {
            return _repository.List(spec);
        }

        public T Create<T>(T dataItem) where T : DataEntity
        {
            return _repository.Create(dataItem);
        }

        public T Update<T>(T dataItem) where T : DataEntity
        {
            return _repository.Update(dataItem);
        }

        public void Update<T>(List<T> dataItemList) where T : DataEntity
        {
            _repository.Update(dataItemList);
        }

        public void Create<T>(List<T> dataItemList) where T : DataEntity
        {
            _repository.Create(dataItemList);
        }
    }
}