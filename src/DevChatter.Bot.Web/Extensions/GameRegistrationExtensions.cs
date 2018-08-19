using DevChatter.Bot.Core.Commands;
using DevChatter.Bot.Core.Games.Hangman;
using DevChatter.Bot.Core.Games.Heist;
using DevChatter.Bot.Core.Games.Quiz;
using DevChatter.Bot.Core.Games.RockPaperScissors;
using DevChatter.Bot.Core.Games.Roulette;
using Microsoft.Extensions.DependencyInjection;

namespace DevChatter.Bot.Web.Extensions
{
    public static class GameRegistrationExtensions
    {
        public static IServiceCollection AddAllGames(this IServiceCollection services)
        {
            services.AddHeistGame()
                .AddHangmanGame()
                .AddQuizGame()
                .AddRouletteGame()
                .AddRockPaperScissors();

            return services;
        }

        public static IServiceCollection AddHeistGame(this IServiceCollection services)
        {
            services.AddSingleton<IBotCommand, HeistCommand>();
            services.AddSingleton<HeistGame>();

            return services;
        }

        public static IServiceCollection AddHangmanGame(this IServiceCollection services)
        {
            services.AddSingleton<HangmanGame>();
            services.AddSingleton<IBotCommand, HangmanCommand>();

            return services;
        }

        public static IServiceCollection AddRockPaperScissors(this IServiceCollection services)
        {
            services.AddSingleton<RockPaperScissorsGame>();
            services.AddSingleton<IBotCommand, RockPaperScissorsCommand>();

            return services;
        }

        public static IServiceCollection AddQuizGame(this IServiceCollection services)
        {
            services.AddSingleton<IBotCommand, QuizCommand>();
            services.AddSingleton<QuizGame>();

            return services;
        }

        public static IServiceCollection AddRouletteGame(this IServiceCollection services)
        {
            services.AddSingleton<IBotCommand, RouletteCommand>();

            return services;
        }
    }
}
