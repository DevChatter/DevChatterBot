using System;
using DevChatter.Bot.Core.Messaging;

namespace DevChatter.Bot.Core.Data
{
    public abstract class DataItem
    {
        public Guid Id { get; set; }
        public DataItemStatus DataItemStatus { get; protected set; } = DataItemStatus.Draft;
    }
}