using Autofac;
using DevChatter.Bot.Core.BotModules.DuelingModule;
using DevChatter.Bot.Core.BotModules.VotingModule;

namespace DevChatter.Bot.Core
{
    public class CoreRegistrationModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<DuelingSystem>();
            builder.RegisterType<VotingSystem>();
        }
    }
}
