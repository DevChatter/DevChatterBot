using Autofac;
using DevChatter.Bot.Core.Games.Hangman;
using DevChatter.Bot.Core.Games.Heist;
using DevChatter.Bot.Core.Games.Quiz;
using DevChatter.Bot.Core.Games.RockPaperScissors;

namespace DevChatter.Bot.Web.Extensions
{
    public static class GameRegistrationExtensions
    {
        public static ContainerBuilder AddAllGames(this ContainerBuilder builder)
        {
            builder.RegisterType<HeistGame>().SingleInstance();
            builder.RegisterType<HangmanGame>().SingleInstance();
            builder.RegisterType<RockPaperScissorsGame>().SingleInstance();
            builder.RegisterType<QuizGame>().SingleInstance();

            return builder;
        }
    }
}
