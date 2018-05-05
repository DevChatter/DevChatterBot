using System;
using System.Collections.Generic;
using System.Linq;
using DevChatter.Bot.Core.Data;
using DevChatter.Bot.Core.Data.Model;
using DevChatter.Bot.Core.Events.Args;

namespace DevChatter.Bot.Core.Commands.Operations
{
    public class TopCommandsOperation : BaseCommandOperation
    {
        private readonly IRepository _repository;

        public TopCommandsOperation(IRepository repository)
        {
            _repository = repository;
        }

        public const string NO_DATA_MESSAGE = "There's no analytics data yet";

        public override List<string> OperandWords { get; } = new List<string> { "top" };
        public override string HelpText { get; } = $"Call \"!command top\" to see the most used commands.";
        public override string TryToExecute(CommandReceivedEventArgs eventArgs)
        {
            List<string> commandUseCountStrings = _repository.List<CommandUsageEntity>()
                .GroupBy(usage => usage.FullTypeName)
                .OrderByDescending(grp => grp.Count())
                .Take(5)
                .Select(grp => $"{grp.Key.Split('.').Last()}: {grp.Count()}")
                .ToList();

            if (!commandUseCountStrings.Any())
            {
                return NO_DATA_MESSAGE;
            }

            string commandPart = string.Join(", ", commandUseCountStrings);
            return $"The most used commands are - {commandPart}";
        }
    }
}
