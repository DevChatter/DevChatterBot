using System;
using NodaTime;

namespace DevChatter.Bot.Core.Data.Model
{
    public class ScheduleEntity : DataEntity
    {
        public Instant Instant => Instant.FromDateTimeOffset(ExampleDateTime);
        public DateTimeOffset ExampleDateTime { get; set; }
    }
}
