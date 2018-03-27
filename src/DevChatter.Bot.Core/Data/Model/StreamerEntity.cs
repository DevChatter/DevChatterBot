using System;

namespace DevChatter.Bot.Core.Data.Model
{
    public class StreamerEntity : DataEntity
    {
        public string ChannelName { get; set; }
        public DateTime DateAdded { get; set; } = DateTime.UtcNow;
        public int TimesShoutedOut { get; set; } = 0;
    }
}