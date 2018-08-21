using Autofac;
using DevChatter.Bot.Core.Games.Hangman;
using DevChatter.Bot.Core.Games.Heist;
using DevChatter.Bot.Core.Games.Quiz;
using DevChatter.Bot.Core.Games.RockPaperScissors;
using DevChatter.Bot.Core.Games.Roulette;

namespace DevChatter.Bot.Web.Extensions
{
    public static class GameRegistrationExtensions
    {
        public static ContainerBuilder AddAllGames(this ContainerBuilder builder)
        {
            builder.AddHeistGame()
                .AddHangmanGame()
                .AddQuizGame()
                .AddRouletteGame()
                .AddRockPaperScissors();

            return builder;
        }

        public static ContainerBuilder AddHeistGame(this ContainerBuilder builder)
        {
            builder.RegisterType<HeistCommand>()
                .AsImplementedInterfaces()
                .SingleInstance();
            builder.RegisterType<HeistGame>().SingleInstance();

            return builder;
        }

        public static ContainerBuilder AddHangmanGame(this ContainerBuilder builder)
        {
            builder.RegisterType<HangmanGame>().SingleInstance();
            builder.RegisterType<HangmanCommand>()
                .AsImplementedInterfaces().SingleInstance();

            return builder;
        }

        public static ContainerBuilder AddRockPaperScissors(this ContainerBuilder builder)
        {
            builder.RegisterType<RockPaperScissorsGame>().SingleInstance();
            builder.RegisterType<RockPaperScissorsCommand>()
                .AsImplementedInterfaces().SingleInstance();

            return builder;
        }

        public static ContainerBuilder AddQuizGame(this ContainerBuilder builder)
        {
            builder.RegisterType<QuizCommand>()
                .AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<QuizGame>().SingleInstance();

            return builder;
        }

        public static ContainerBuilder AddRouletteGame(this ContainerBuilder builder)
        {
            builder.RegisterType<RouletteCommand>()
                .AsImplementedInterfaces().SingleInstance();

            return builder;
        }
    }
}
