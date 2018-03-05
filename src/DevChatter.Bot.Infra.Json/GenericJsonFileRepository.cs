using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DevChatter.Bot.Core;
using DevChatter.Bot.Core.Data;
using Newtonsoft.Json;

namespace DevChatter.Bot.Infra.Json
{
    public class GenericJsonFileRepository : IRepository
    {
        public List<T> List<T>(ISpecification<T> spec)
        {
            string filePath = $"FileDataStore\\{typeof(T).Name}.json";
            string fullText = File.ReadAllText(filePath);

            var list = JsonConvert.DeserializeObject<List<T>>(fullText, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Objects
            });

            return list?.Where(x => spec.Criteria.Compile().Invoke(x))?.ToList();
        }
    }
}
