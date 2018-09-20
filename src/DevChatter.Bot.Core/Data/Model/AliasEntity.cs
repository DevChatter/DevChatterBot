using System.Collections.Generic;

namespace DevChatter.Bot.Core.Data.Model
{
    public class AliasEntity : DataEntity
    {
        public CommandEntity Command { get; set; }
        public string Word { get; set; }
        public List<AliasArgumentEntity> Arguments { get; set; }
            = new List<AliasArgumentEntity>();
    }
}
