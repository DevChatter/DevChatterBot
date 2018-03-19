using System;

namespace DevChatter.Bot.Core.Model
{
    public class QuoteEntity : DataItem
    {
        public int QuoteId { get; set; }
        public string Message { get; set; }
        public string Author { get; set; }
        public string AddedBy { get; set; }
        public DateTime DateAdded { get; set; }
    }
}