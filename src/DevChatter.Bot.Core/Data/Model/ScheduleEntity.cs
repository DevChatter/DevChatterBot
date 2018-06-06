using System;
using System.ComponentModel;
using NodaTime;

namespace DevChatter.Bot.Core.Data.Model
{
    public class ScheduleEntity : DataEntity
    {
        public Instant Instant => Instant.FromDateTimeOffset(ExampleDateTime);
        [DisplayName("Stream Example Date Time")]
        public DateTimeOffset ExampleDateTime { get; set; }
    }
}
