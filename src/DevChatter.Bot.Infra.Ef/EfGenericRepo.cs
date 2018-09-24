using DevChatter.Bot.Core.Data;
using DevChatter.Bot.Core.Data.Model;
using DevChatter.Bot.Core.Data.Specifications;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace DevChatter.Bot.Infra.Ef
{
    public class EfGenericRepo : IRepository
    {
        private readonly AppDataContext _db;

        public EfGenericRepo(AppDataContext db)
        {
            _db = db;
        }

        public T Single<T>(ISpecification<T> spec) where T : DataEntity
        {
            IQueryable<T> setWithIncludes = SetWithIncludes(spec);
            return setWithIncludes.SingleOrDefault(spec.Criteria);
        }

        public List<T> List<T>(ISpecification<T> spec) where T : DataEntity
        {
            return spec != null
                ? SetWithIncludes(spec).Where(spec.Criteria).ToList()
                : _db.Set<T>().ToList();
        }

        public T Create<T>(T dataItem) where T : DataEntity
        {
            _db.Set<T>().Add(dataItem);
            _db.SaveChanges();

            return dataItem;
        }

        public T Update<T>(T dataItem) where T : DataEntity
        {
            _db.Set<T>().Update(dataItem);
            _db.SaveChanges();

            return dataItem;
        }

        public void Update<T>(List<T> dataItemList) where T : DataEntity
        {
            _db.Set<T>().UpdateRange(dataItemList);
            _db.SaveChanges();
        }

        public void Create<T>(List<T> dataItemList) where T : DataEntity
        {
            _db.Set<T>().AddRange(dataItemList);
            _db.SaveChanges();
        }

        public void Remove<T>(T dataItem) where T : DataEntity
        {
            _db.Set<T>().Remove(dataItem);
            _db.SaveChanges();
        }

        public void Remove<T>(List<T> dataItems) where T : DataEntity
        {
            _db.Set<T>().RemoveRange(dataItems);
            _db.SaveChanges();
        }

        private IQueryable<T> SetWithIncludes<T>(ISpecification<T> spec) where T : DataEntity
        {
            var withExpressionIncludes = spec?.Includes
                .Aggregate(_db.Set<T>().AsQueryable(),
                    (queryable, include) => queryable.Include(include));

            var withAllIncludes = spec?.IncludeStrings
                .Aggregate(withExpressionIncludes,
                    (queryable, include) => queryable.Include(include));

            return withAllIncludes;
        }
    }
}
