using System;
using DevChatter.Bot.Core;

namespace DevChatter.Bot
{
    public class ConsoleChatClient : IChatClient
    {
        public void SendMessage(string message)
        {
            Console.WriteLine(message);
        }
    }
}