using DevChatter.Bot.Core.Data;
using DevChatter.Bot.Core.Data.Model;
using DevChatter.Bot.Core.Data.Specifications;
using DevChatter.Bot.Core.Events.Args;
using System;
using System.Collections.Generic;

namespace DevChatter.Bot.Core.Commands.Operations
{
    public class GenericDeleteOperation<T> : BaseCommandOperation where T : DataEntity
    {
        protected IRepository _repository;
        protected Func<CommandReceivedEventArgs, ISpecification<T>> _specFunction;
        protected UserRole _requiredRole;

        public GenericDeleteOperation(IRepository repository, UserRole requiredRole,
            Func<CommandReceivedEventArgs, ISpecification<T>> specFunction)
        {
            _repository = repository;
            _specFunction = specFunction;
            _requiredRole = requiredRole;
        }

        public override List<string> OperandWords { get; } = new List<string> {"del", "rem", "remove", "delete"};

        public override string HelpText => "";

        public override string TryToExecute(CommandReceivedEventArgs eventArgs)
        {
            var dataItem = _repository.Single(_specFunction(eventArgs));

            if (dataItem == null)
            {
                return $"The {typeof(T).Name} you tried to delete does not exist.";
            }

            _repository.Remove(dataItem);

            return $"The {typeof(T).Name} has been deleted.";
        }
    }
}
