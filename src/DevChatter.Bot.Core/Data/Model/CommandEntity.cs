using System.Collections.Generic;

namespace DevChatter.Bot.Core.Data.Model
{
    public class CommandEntity : DataEntity
    {
        public string CommandWord { get; set; }
        public string FullTypeName { get; set; }
        public List<AliasEntity> Aliases { get; set; } = new List<AliasEntity>();
        public bool IsEnabled { get; set; } = true;
        public UserRole RequiredRole { get; set; }
    }
}
