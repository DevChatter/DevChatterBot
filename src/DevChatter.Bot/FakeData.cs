using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using DevChatter.Bot.Core.Messaging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace DevChatter.Bot
{
    public static class FakeData
    {
        private static List<IAutomatedMessage> GetIAutomatedMessage()
        {
            var automatedMessages = new List<IAutomatedMessage> {
                new IntervalTriggeredMessage(1, "Hello and welcome! I hope you're enjoying the stream! Feel free to follow along, make suggestions, ask questions, or contribute! And make sure you click the follow button to know when the next stream is!", DataItemStatus.Active),
                new IntervalTriggeredMessage(1,"foo", DataItemStatus.Draft),
                new IntervalTriggeredMessage(2,"bar", DataItemStatus.Disabled),
            };
            return automatedMessages;
        }

        private static List<ICommandMessage> GetICommandMessages()
        {
            return new List<ICommandMessage>
            {
                new StaticCommandResponseMessage("coins", "Coins?!?! I think you meant !points", DataItemStatus.Active),
            };
        }

        public static void Initialize()
        {
            StoreData(GetIAutomatedMessage());

            StoreData(GetICommandMessages());
        }

        private static void StoreData<T>(List<T> dataToStore)
        {
            string serializedJson = JsonConvert.SerializeObject(dataToStore, Formatting.Indented, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Objects,
                TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Full,
            });

            WriteDataToFilesystem(typeof(T).Name, serializedJson);
        }

        private static void WriteDataToFilesystem(string dataTypeName, string textToWrite)
        {
            string filePath = $"FileDataStore\\{dataTypeName}.json";
            if (!Directory.Exists("FileDataStore"))
            {
                Directory.CreateDirectory("FileDataStore");
            }

            if (!File.Exists(filePath))
            {
                File.Create(filePath);
            }

            File.WriteAllText(filePath, textToWrite);
        }
    }
}