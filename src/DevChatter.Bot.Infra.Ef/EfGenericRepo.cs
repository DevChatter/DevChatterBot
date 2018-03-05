using System.Collections.Generic;
using System.Linq;
using DevChatter.Bot.Core;
using DevChatter.Bot.Core.Data;

namespace DevChatter.Bot.Infra.Ef
{
    public class EfGenericRepo : IRepository
    {
        private readonly AppDataContext _db;

        public EfGenericRepo(AppDataContext db)
        {
            _db = db;
        }

        public List<T> List<T>(ISpecification<T> spec) where T : DataItem
        {
            return _db.Set<T>().Where(spec.Criteria).ToList();
        }

        public T Create<T>(T dataItem) where T : DataItem
        {
            _db.Set<T>().Add(dataItem);
            _db.SaveChanges();

            return dataItem;
        }

        public List<T> Create<T>(List<T> dataItemList) where T : DataItem
        {
            _db.Set<T>().AddRange(dataItemList);
            _db.SaveChanges();

            return dataItemList;
        }
    }
}