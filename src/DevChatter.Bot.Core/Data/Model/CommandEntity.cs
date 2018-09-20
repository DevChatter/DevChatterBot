using System.Collections.Generic;

namespace DevChatter.Bot.Core.Data.Model
{
    public class CommandEntity : DataEntity
    {
        public string CommandWord { get; set; }
        public string FullTypeName { get; set; }
        public bool IsPrimary { get; set; }
        public List<AliasArgumentEntity> Arguments { get; set; } = new List<AliasArgumentEntity>();
    }
}
