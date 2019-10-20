using Autofac;
using DevChatter.Bot.Core.Automation;
using DevChatter.Bot.Core.Events;

namespace DevChatter.Bot.Web.Modules
{
    public class CurrencyModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<CurrencyGenerator>()
                .AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<CurrencyUpdate>()
                .As<IIntervalAction>()
                .SingleInstance();
        }
    }
}
