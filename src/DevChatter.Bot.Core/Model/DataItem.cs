using System;

namespace DevChatter.Bot.Core.Model
{
    public abstract class DataItem
    {
        public Guid Id { get; set; }
        public DataItemStatus DataItemStatus { get; protected set; } = DataItemStatus.Draft;
    }
}