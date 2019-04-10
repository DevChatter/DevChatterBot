using DevChatter.Bot.Core.Commands;
using DevChatter.Bot.Core.Data;
using DevChatter.Bot.Core.Data.Model;
using DevChatter.Bot.Core.Games.Roulette;
using DevChatter.Bot.Core.Settings;
using DevChatter.Bot.Infra.Ef;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using DevChatter.Bot.Core.Games.Hangman;
using DevChatter.Bot.Core.Games.RockPaperScissors;

namespace DevChatter.Bot.Web.Setup
{
    public static class SetUpDatabase
    {
        public static IRepository SetUpRepository(BotConfiguration botConfig)
        {
            DbContextOptions<AppDataContext> options = new DbContextOptionsBuilder<AppDataContext>()
                .UseSqlServer(botConfig.ConnectionStrings.DefaultDatabase)
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
            if (!repository.List<ScheduleEntity>().Any())
            {
                repository.Create(GetDevChatterSchedule());
            }

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

            if (!repository.List<HangmanWord>().Any())
            {
                repository.Create(GetInitialHangmanWords());
            }

            if (!repository.List<QuizQuestion>().Any())
            {
                repository.Create(GetInitialQuizQuestions());
            }

            if (!repository.List<CanvasProperties>().Any())
            {
                repository.Create(GetInitialCanvasProperties());
            }

            CreateDefaultSettingsIfNeeded(repository);

            // TODO: Remove static call, so it's more testable.
            CommandDataInitializer.UpdateCommandData(repository);
        }

        private static List<CanvasProperties> GetInitialCanvasProperties()
        {
            var canvasProperties = new List<CanvasProperties>
            {
                new CanvasProperties
                {
                    CanvasId = "hangmanCanvas",
                    Height = 300,
                    Width = 800,
                    TopY = 780,
                    LeftX = 560
                },
                new CanvasProperties
                {
                    CanvasId = "animationCanvas",
                    Height = 1080,
                    Width = 1920,
                    TopY = 0,
                    LeftX = 0
                },
                new CanvasProperties
                {
                    CanvasId = "votingCanvas",
                    Height = 1080,
                    Width = 1920,
                    TopY = 0,
                    LeftX = 0
                },
            };

            return canvasProperties;
        }

        private static void CreateDefaultSettingsIfNeeded(IRepository repository)
        {
            var settingsFactory = new SettingsFactory(repository);
            settingsFactory.CreateDefaultSettingsIfNeeded<RouletteSettings>();
            settingsFactory.CreateDefaultSettingsIfNeeded<CurrencySettings>();
            settingsFactory.CreateDefaultSettingsIfNeeded<HangmanSettings>();
            settingsFactory.CreateDefaultSettingsIfNeeded<RockPaperScissorsSettings>();
        }

        private static List<QuizQuestion> GetInitialQuizQuestions()
        {
            return new List<QuizQuestion>
            {
                new QuizQuestion
                {
                    MainQuestion = "Who is the best C# Twitch Streamer?",
                    Hint1 = "We aren't wearing hats...",
                    Hint2 = "Brendan is modest enough, wouldn't you say?",
                    CorrectAnswer = "DevChatter",
                    WrongAnswer1 = "CSharpFritz",
                    WrongAnswer2 = "Certainly not any of these other choices Kappa ",
                    WrongAnswer3 = "robbiew_yt",
                },
                new QuizQuestion
                {
                    MainQuestion = "Which of these is NOT valid in C#?",
                    Hint1 = "Tuples are OK by me.",
                    Hint2 = "Should I give three hints?",
                    CorrectAnswer = "int c, int d = 3;",
                    WrongAnswer1 = "int x = 4;",
                    WrongAnswer2 = "(int x, int y) = (1,2);",
                    WrongAnswer3 = "int a = 5, b = 6;",
                },
            };
        }

        private static List<ScheduleEntity> GetDevChatterSchedule()
        {
            return new List<ScheduleEntity>
            {
                new ScheduleEntity {ExampleDateTime = new DateTimeOffset(2018, 5, 7, 18, 0, 0, TimeSpan.Zero)},
                new ScheduleEntity {ExampleDateTime = new DateTimeOffset(2018, 5, 8, 18, 0, 0, TimeSpan.Zero)},
                new ScheduleEntity {ExampleDateTime = new DateTimeOffset(2018, 5, 10, 16, 0, 0, TimeSpan.Zero)},
                new ScheduleEntity {ExampleDateTime = new DateTimeOffset(2018, 5, 12, 17, 0, 0, TimeSpan.Zero)}
            };
        }

        private static List<HangmanWord> GetInitialHangmanWords()
        {
            return new List<HangmanWord>
            {
                new HangmanWord("apple"),
                new HangmanWord("banana"),
                new HangmanWord("orange"),
                new HangmanWord("mango"),
                new HangmanWord("watermellon"),
                new HangmanWord("grapes"),
                new HangmanWord("pizza"),
                new HangmanWord("pasta"),
                new HangmanWord("pepperoni"),
                new HangmanWord("cheese"),
                new HangmanWord("mushroom"),
                new HangmanWord("csharp"),
                new HangmanWord("javascript"),
                new HangmanWord("cplusplus"),
                new HangmanWord("nullreferenceexception"),
                new HangmanWord("parameter"),
                new HangmanWord("argument")
            };
        }

        private static List<IntervalMessage> GetIntervalMessages()
        {
            var automatedMessages = new List<IntervalMessage>
            {
                new IntervalMessage("Hello and welcome! I hope you're enjoying the stream! Feel free to follow along, make suggestions, ask questions, or contribute! And make sure you click the follow button to know when the next stream is!")
            };
            return automatedMessages;
        }

        private static List<SimpleCommand> GetSimpleCommands()
        {
            return new List<SimpleCommand>
            {
                new SimpleCommand("discord", "Hey! Checkout out our Discord here https://discord.gg/aQry9jG"),
                new SimpleCommand("github", "Check out our GitHub repositories here https://github.com/DevChatter/"),
                new SimpleCommand("emotes", "These are our current emotes: devchaHype devchaDerp devchaFail "),
                new SimpleCommand("lurk",
                    "[UserDisplayName] is just lurking here, but still thinks you're all awesome!"),
            };
        }

        private static List<QuoteEntity> GetInitialQuotes()
        {
            return new List<QuoteEntity>
            {
                new QuoteEntity
                {
                    QuoteId = 1,
                    DateAdded = new DateTime(2018, 3, 19),
                    AddedBy = "Brendoneus",
                    Author = "DevChatter",
                    Text = "Hello world!"
                },
                new QuoteEntity
                {
                    QuoteId = 2,
                    DateAdded = new DateTime(2018, 3, 19),
                    AddedBy = "Brendoneus",
                    Author = "DevChatter",
                    Text = "Welcome to DevChatter!"
                },
                new QuoteEntity
                {
                    QuoteId = 3,
                    DateAdded = new DateTime(2018, 3, 20),
                    AddedBy = "cragsify",
                    Author = "DevChatter",
                    Text = "I swear it's not rigged!"
                },
            };
        }
    }
}
