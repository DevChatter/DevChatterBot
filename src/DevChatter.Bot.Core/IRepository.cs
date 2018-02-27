using System.Collections.Generic;
using DevChatter.Bot.Core.Data;

namespace DevChatter.Bot.Core
{
    public interface IRepository
    {
        List<T> List<T>(ISpecification<T> spec);
    }
}