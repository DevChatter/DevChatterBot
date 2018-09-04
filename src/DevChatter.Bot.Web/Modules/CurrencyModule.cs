using Autofac;
using DevChatter.Bot.Core.Automation;
using DevChatter.Bot.Core.Commands;
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
            builder.RegisterType<TaxCommand>()
                .AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<GiveCommand>()
                .AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<CoinsCommand>()
                .AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<BonusCommand>()
                .AsImplementedInterfaces().SingleInstance();

        }
    }
}
