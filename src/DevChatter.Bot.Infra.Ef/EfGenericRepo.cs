using System.Collections.Generic;
using System.Linq;
using DevChatter.Bot.Core.Data;
using DevChatter.Bot.Core.Model;
using Microsoft.EntityFrameworkCore;

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
            return _db.Set<T>().SingleOrDefault(spec.Criteria);
        }

        public List<T> List<T>(ISpecification<T> spec) where T : DataEntity
        {
            DbSet<T> dbSet = _db.Set<T>();
            return spec != null ? dbSet.Where(spec.Criteria).ToList() : dbSet.ToList();
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
    }
}