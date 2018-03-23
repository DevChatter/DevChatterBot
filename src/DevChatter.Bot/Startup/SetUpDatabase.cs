using System;
using System.Collections.Generic;
using System.Linq;
using DevChatter.Bot.Core.Commands;
using DevChatter.Bot.Core.Data;
using DevChatter.Bot.Core.Messaging;
using DevChatter.Bot.Core.Model;
using DevChatter.Bot.Infra.Ef;
using Microsoft.EntityFrameworkCore;

namespace DevChatter.Bot.Startup
{
    public static class SetUpDatabase
    {
        public static IRepository SetUpRepository(string connectionString)
        {
            DbContextOptions<AppDataContext> options = new DbContextOptionsBuilder<AppDataContext>()
                .UseSqlServer(connectionString)
                .Options;

            var appDataContext = new AppDataContext(options);

            EnsureDatabase(appDataContext);
            IRepository repository = new EfGenericRepo(appDataContext);
            EnsureInitialData(repository);

            return repository;
        }

        private static void EnsureDatabase(AppDataContext dataContext)
        {
            dataContext.Database.Migrate();
        }

        private static void EnsureInitialData(IRepository repository)
        {
            if (!repository.List<IntervalMessage>().Any())
            {
                repository.Create(GetIntervalMessages());
            }

            if (!repository.List<SimpleCommand>().Any())
            {
                repository.Create(GetSimpleCommands());
            }

            if (!repository.List<QuoteEntity>().Any())
            {
                repository.Create(GetInitialQuotes());
            }
        }

        private static List<IntervalMessage> GetIntervalMessages()
        {
            var automatedMessages = new List<IntervalMessage>
            {
                new IntervalMessage(15,
                    "Hello and welcome! I hope you're enjoying the stream! Feel free to follow along, make suggestions, ask questions, or contribute! And make sure you click the follow button to know when the next stream is!")
            };
            return automatedMessages;
        }

        private static List<SimpleCommand> GetSimpleCommands()
        {
            return new List<SimpleCommand>
            {
                new SimpleCommand("coins", "Coins?!?! I think you meant !points"),
                new SimpleCommand("discord", "Hey! Checkout out our Discord here https://discord.gg/aQry9jG"),
                new SimpleCommand("github", "Check out our GitHub repositories here https://github.com/DevChatter/"),
                new SimpleCommand("emotes", "These are our current emotes: devchaHype devchaDerp devchaFail "),
                new SimpleCommand("lurk", "[UserDisplayName] is just lurking here, but still thinks you're all awesome!"),
            };
        }

        private static List<QuoteEntity> GetInitialQuotes()
        {
            return new List<QuoteEntity>
            {
                new QuoteEntity{QuoteId = 1, DateAdded = new DateTime(2018,3,19),
                    AddedBy = "Brendoneus", Author = "DevChatter", Text = "Hello world!"},
                new QuoteEntity{QuoteId = 2, DateAdded = new DateTime(2018,3,19),
                    AddedBy = "Brendoneus", Author = "DevChatter", Text = "Welcome to DevChatter!"},
                new QuoteEntity{QuoteId = 3, DateAdded = new DateTime(2018,3,20),
                    AddedBy = "cragsify", Author = "DevChatter", Text = "I swear it's not rigged!"},
            };
        }
    }
}