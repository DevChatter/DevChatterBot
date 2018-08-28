using Autofac;
using DevChatter.Bot.Core.Events;

namespace DevChatter.Bot.Core
{
    public class CoreModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<DomainEventHandler>().AsSelf().SingleInstance();
            builder.RegisterType<DomainEventRaiser>().As<IDomainEventRaiser>();
        }
    }
}
