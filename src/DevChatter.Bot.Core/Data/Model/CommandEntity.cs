using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace DevChatter.Bot.Core.Data.Model
{
    public class CommandEntity : DataEntity
    {
        public string CommandWord { get; set; }
        public string FullTypeName { get; set; }
        public List<AliasEntity> Aliases { get; set; } = new List<AliasEntity>();
        public bool IsEnabled { get; set; } = true;

        [JsonConverter(typeof(StringEnumConverter))]
        public UserRole RequiredRole { get; set; } = UserRole.Everyone;
        public TimeSpan Cooldown { get; set; } = TimeSpan.Zero;
        public string HelpText { get; set; }
    }
}
