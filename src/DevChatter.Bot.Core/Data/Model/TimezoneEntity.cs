using System;
using DevChatter.Bot.Core.GoogleApi;

namespace DevChatter.Bot.Core.Data.Model
{
    public class TimezoneEntity : DataEntity
    {
        public string TimezoneName { get; set; }
        public string LookupString { get; set; }
        public float Latitude { get; set; }
        public float Longitude { get; set; }
        public int Offset { get; set; }
        public DateTime DateUpdated { get; set; } = DateTime.UtcNow;

        public TimezoneLookupResult ToTimezoneLookupResult()
        {
            return new TimezoneLookupResult
            {
                Offset = Offset,
                TimezoneName = TimezoneName,
                Success = true,
            };
        }
    }
}
