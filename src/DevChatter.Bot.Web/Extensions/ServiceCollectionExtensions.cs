using System;
using System.Collections.Generic;
using DevChatter.Bot.Core.Commands;
using DevChatter.Bot.Core.Data;
using DevChatter.Bot.Core.Data.Specifications;
using Microsoft.Extensions.DependencyInjection;

namespace DevChatter.Bot.Web.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSimpleCommandsFromRepository(
            this IServiceCollection services,
            IRepository repository)
        {
            List<SimpleCommand> simpleCommands = repository.List(CommandPolicy.All());

            foreach (SimpleCommand simpleCommand in simpleCommands)
            {
                services.AddSingleton<IBotCommand>(simpleCommand);
            }

            return services;
        }
    }
}
