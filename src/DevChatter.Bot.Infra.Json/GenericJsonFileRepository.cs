using System;
using System.Collections.Generic;
using DevChatter.Bot.Core;
using DevChatter.Bot.Core.Data;

namespace DevChatter.Bot.Infra.Json
{
    public class GenericJsonFileRepository : IRepository
    {
        public List<T> List<T>(ISpecification<T> spec)
        {
            throw new NotImplementedException();
        }
    }
}
