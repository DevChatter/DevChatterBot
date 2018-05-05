using System;

namespace DevChatter.Bot.Core.Data.Model
{
    public class CommandUsageEntity : DataEntity
    {
        protected CommandUsageEntity()
        {
        }

        public CommandUsageEntity(string commandWord, string fullTypeName,
            string userId, string userDisplayName)
        {
            CommandWord = commandWord;
            FullTypeName = fullTypeName;
            UserId = userId;
            UserDisplayName = userDisplayName;
        }

        public string CommandWord { get; set; }
        public string FullTypeName { get; set; }
        public string UserId { get; set; }
        public string UserDisplayName { get; set; }
        public DateTimeOffset DateTimeUsed { get; set; } = DateTimeOffset.UtcNow;
    }
}
