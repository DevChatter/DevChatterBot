using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DevChatter.Bot.Core.Commands;
using DevChatter.Bot.Core.Data;
using DevChatter.Bot.Core.Data.Model;
using DevChatter.Bot.Core.Data.Specifications;

namespace DevChatter.Bot.Web.Setup
{
    public static class SetUpCommandData
    {

        public static void UpdateCommandData(IRepository repository)
        {
            try
            {
                var (wordsToAdd, wordsToRemove) = GetCommandWordChanges(repository);
                if (wordsToAdd.Any())
                {
                    repository.Create(wordsToAdd);
                }

                if (wordsToRemove.Any())
                {
                    repository.Remove(wordsToRemove);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private static (List<CommandEntity> WordsToAdd, List<CommandEntity> WordsToRemove) GetCommandWordChanges(IRepository repository)
        {
            const string conventionSuffix = "Command";

            // Access the assembly to make sure it's loaded
            Assembly assembly = typeof(BlastCommand).Assembly;

            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();

            IEnumerable<TypeInfo> allCommandTypes = assemblies.SelectMany(x => x.DefinedTypes);

            var concreteCommands = allCommandTypes
                .Where(x => typeof(IBotCommand).IsAssignableFrom(x))
                .Where(x => !x.IsAbstract)
                .Where(x => !x.IsSubclassOf(typeof(DataEntity)))
                .Where(x => x.FullName.EndsWith(conventionSuffix))
                .ToList();

            List<CommandEntity> commandEntities = repository.List(CommandPolicy.All());
            List<string> commandTypes = commandEntities.Select(x => x.FullTypeName).ToList();

            List<CommandEntity> missingDefaults = concreteCommands
                .Select(commandType => new CommandEntity
                {
                    CommandWord = commandType.Name.Substring(0, commandType.Name.Length - conventionSuffix.Length),
                    FullTypeName = commandType.FullName,
                    // TODO: Add the default RoleRequired
                })
                .Where(x => !commandTypes.Contains(x.FullTypeName))
                .ToList();

            var entitiesToRemove = commandEntities.Where(x => concreteCommands.All(c => c.FullName != x.FullTypeName)).ToList();
            return (missingDefaults, entitiesToRemove);
        }
    }
}
