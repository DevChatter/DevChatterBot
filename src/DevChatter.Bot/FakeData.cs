using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using DevChatter.Bot.Core.Messaging;
using Newtonsoft.Json;

namespace DevChatter.Bot
{
    public static class FakeData
    {
        private static List<IntervalTriggeredMessage> GetIAutomatedMessage()
        {
            var automatedMessages = new List<IntervalTriggeredMessage> {
                new IntervalTriggeredMessage(15, "Hello and welcome! I hope you're enjoying the stream! Feel free to follow along, make suggestions, ask questions, or contribute! And make sure you click the follow button to know when the next stream is!", DataItemStatus.Active),
                new IntervalTriggeredMessage(1,"foo", DataItemStatus.Draft),
                new IntervalTriggeredMessage(2,"bar", DataItemStatus.Disabled),
            };
            return automatedMessages;
        }

        public static void Initialize()
        {
            var jsonSerializer = new JsonSerializer{TypeNameHandling = TypeNameHandling.Auto};

            List<IntervalTriggeredMessage> automatedMessages = GetIAutomatedMessage();
            var stringBuilder = new StringBuilder();
            var stringWriter = new StringWriter(stringBuilder);
            jsonSerializer.Serialize(stringWriter, automatedMessages);
            string filePath = $"FileDataStore\\{nameof(IntervalTriggeredMessage)}.json";
            if (!Directory.Exists("FileDataStore"))
            {
                Directory.CreateDirectory("FileDataStore");
            }
            if (!File.Exists(filePath))
            {
                File.Create(filePath);
            }
            File.WriteAllText(filePath, stringBuilder.ToString());
        }
    }
}