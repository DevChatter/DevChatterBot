using System;

namespace DevChatter.Bot.Core.Data.Model
{
    public class CommandUsageEntity : DataEntity
    {
        public string CommandWord { get; set; }
        public string FullTypeName { get; set; }
        public string UserDisplayName { get; set; }
        public DateTimeOffset DateTimeUsed { get; set; }
    }
}
