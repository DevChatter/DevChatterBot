using System;
using Autofac;
using DevChatter.Bot.Core;
using DevChatter.Bot.Startup;

namespace DevChatter.Bot
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("Initializing the Bot...");

            var botConfiguration = SetUpConfig.InitializeConfiguration();
            var container = SetUpBot.NewBotDependencyContainer(botConfiguration);
            WaitForCommands(container);
        }

        private static void WaitForCommands(IContainer container)
        {
            Console.WriteLine("==============================");
            Console.WriteLine("Available bot commands : start, stop, restart, exit");
            Console.WriteLine("==============================");

            ILifetimeScope scope = null;
            try
            {
                IBotMain botMain = null;
                var command = "start";
                while (true)
                {
                    switch (command)
                    {
                        case "stop":
                            Stop(scope, botMain);
                            break;

                        case "start":
                            Start(container, out scope, out botMain);
                            break;

                        case "restart":
                            Stop(scope, botMain);
                            Start(container, out scope, out botMain);
                            break;

                        case "exit":
                            return;

                        default:
                        {
                            Console.WriteLine($"{command} is not a valid command");
                            break;
                        }
                    }

                    Console.Write("Bot:");
                    command = Console.ReadLine();
                }
            }
            finally
            {
                scope?.Dispose();
            }
        }

        private static void Stop(ILifetimeScope scope, IBotMain botMain)
        {
            Console.WriteLine("Bot stopping....");
            botMain?.Stop();
            scope?.Dispose();
            Console.WriteLine("Bot stopped");
            Console.WriteLine("==============================");
        }

        private static void Start(IContainer container, out ILifetimeScope scope, out IBotMain botMain)
        {
            Console.WriteLine("Bot starting....");
            scope = container.BeginLifetimeScope();
            botMain = scope.Resolve<IBotMain>();
            botMain.Run();
            Console.WriteLine("Bot started");
            Console.WriteLine("==============================");
        }
    }
}
