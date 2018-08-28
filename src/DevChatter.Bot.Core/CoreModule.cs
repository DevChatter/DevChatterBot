using Autofac;
using DevChatter.Bot.Core.Events;

namespace DevChatter.Bot.Core
{
    public class CoreModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType(typeof(DomainEventHandler<>))
                .As(typeof(DomainEventHandler<>))
                .As<BaseDomainEventHandler>()
                .SingleInstance();

            builder.RegisterType<DomainEventRaiser>()
                .As<IDomainEventRaiser>();
        }
    }
}
