using Autofac;
using DevChatter.Bot.Core.Automation;
using DevChatter.Bot.Core.Data;
using DevChatter.Bot.Core.Data.Model;
using DevChatter.Bot.Core.Systems.Chat;

namespace DevChatter.Bot.Web.Modules
{
    public class AutomatedMessageModule : Module
    {
        private readonly IRepository _repository;

        public AutomatedMessageModule(IRepository repository)
        {
            _repository = repository;
        }

        protected override void Load(ContainerBuilder builder)
        {
            var messages = _repository.List<IntervalMessage>();
            foreach (IntervalMessage intervalMessage in messages)
            {
                builder.RegisterType<AutomatedMessage>()
                    .WithParameter("intervalMessage", intervalMessage)
                    .As<IIntervalAction>();
            }
        }
    }
}
