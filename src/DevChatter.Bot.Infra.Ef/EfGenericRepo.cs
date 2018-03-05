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
    }
}