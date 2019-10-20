using Autofac;

namespace DevChatter.Bot.Web.Modules
{
    public class ConfigurationModule : Module
    {
        private readonly BotConfiguration _configuration;

        public ConfigurationModule(BotConfiguration configuration)
        {
            _configuration = configuration;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterInstance(_configuration.TwitchClientSettings)
                .AsSelf().SingleInstance();

            builder.RegisterInstance(_configuration.CommandHandlerSettings)
                .AsSelf().SingleInstance();

            builder.RegisterInstance(_configuration.GoogleCloudSettings)
                .AsSelf().SingleInstance();
        }
    }
}
