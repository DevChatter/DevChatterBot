using System;

namespace DevChatter.Bot.Core.Data.Model
{
    public class Timezones
    {
        public string TimezoneName { get; set; }
        public string LookupString { get; set; }
        public float Latitude { get; set; }
        public float Longitude { get; set; }
        public int Offset { get; set; }
        public DateTime DateUpdated { get; set; }
    }
}
