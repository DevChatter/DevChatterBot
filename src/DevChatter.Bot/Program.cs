using System;
using System.Collections.Generic;
using DevChatter.Bot.Core;
using DevChatter.Bot.Infra.Twitch;
using Microsoft.Extensions.Configuration;


namespace DevChatter.Bot
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Initializing the Bot...");

            var chatClients = new List<IChatClient> { new ConsoleChatClient(), new TwitchChatClient() };

            Console.WriteLine("To exit, press [Ctrl]+c");

            var botMain = new BotMain(chatClients);
            botMain.Run();
        }
    }
}
