using Autofac;
using DevChatter.Bot.Core.Systems.Chat;
using DevChatter.Bot.Infra.Discord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevChatter.Bot.Web.Modules
{
    public class DiscordModule : Module
    {
        private readonly DiscordClientSettings _discordClientSettings;

        public DiscordModule(DiscordClientSettings discordClientSettings)
        {
            _discordClientSettings = discordClientSettings;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<DiscordChatClient>()
                .WithParameter("settings", _discordClientSettings)
                .As<IChatClient>().AsSelf().SingleInstance();
        }
    }
}
