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
            var serializer = new JsonSerializer();
            string filePath = $"FileDataStore\\{typeof(T).Name}.json";
            using (StreamReader file = File.OpenText(filePath))
            {
                var jsonTextReader = new JsonTextReader(file);
                var list = serializer.Deserialize<List<T>>(jsonTextReader);

                return list.Where(x => spec.Criteria.Compile().Invoke(x)).ToList();
            }
        }
    }
}
