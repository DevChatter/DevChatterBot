using Autofac;

namespace DevChatter.Bot.Infra.Discord
{
    public class DevChatterBotDiscordModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<DiscordChatClient>().AsImplementedInterfaces().SingleInstance();
        }
    }
}
