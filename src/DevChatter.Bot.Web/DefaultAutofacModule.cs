using Autofac;
using DevChatter.Bot.Core;
using DevChatter.Bot.Core.Automation;
using DevChatter.Bot.Core.Commands.Trackers;
using DevChatter.Bot.Core.Data;
using DevChatter.Bot.Core.Events;
using DevChatter.Bot.Core.Messaging;
using DevChatter.Bot.Core.Systems.Streaming;
using DevChatter.Bot.Core.Util;
using DevChatter.Bot.Infra.Ef;
using DevChatter.Bot.Infra.GoogleApi;
using DevChatter.Bot.Infra.Web;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;


namespace DevChatter.Bot.Web
{
    public class DefaultAutofacModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<EfGenericRepo>()
                .AsImplementedInterfaces();

            builder.RegisterType<GoogleApiTimezoneLookup>().AsImplementedInterfaces();

            builder.RegisterType<EfCacheLayer>().AsImplementedInterfaces();

            builder.RegisterType<AutomationSystem>()
                .As<IAutomatedActionSystem>().SingleInstance();

            builder.RegisterType<BotMain>().AsSelf().SingleInstance();

            builder.RegisterType<DevChatterBotBackgroundWorker>()
                .As<IHostedService>();


            builder.RegisterGeneric(typeof(Logger<>))
                .As(typeof(ILogger<>)).SingleInstance();

            builder.RegisterGeneric(typeof(LoggerAdapter<>))
                .As(typeof(ILoggerAdapter<>)).SingleInstance();

            builder.RegisterGeneric(typeof(List<>))
                .As(typeof(IList<>)).SingleInstance();

            builder.RegisterGeneric(typeof(Lazier<>))
                .As(typeof(Lazy<>)).InstancePerRequest();

            builder.RegisterType<CommandHandler>()
                .As<ICommandHandler>().SingleInstance();

            builder.RegisterType<ChatUserCollection>()
                .As<IChatUserCollection>()
                .SingleInstance();

            builder.RegisterType<SettingsFactory>()
                .As<ISettingsFactory>();

            builder.RegisterType<CommandCooldownTracker>()
                .As<ICommandUsageTracker>();

            builder.RegisterType<SystemClock>()
                .As<IClock>();

            builder.RegisterType<StreamingSystem>()
                .AsImplementedInterfaces().SingleInstance();

            builder.RegisterType<AnimationDisplayNotification>()
                .AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<HangmanDisplayNotification>()
                .AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<VotingDisplayNotification>()
                .AsImplementedInterfaces().SingleInstance();

            builder.RegisterType<IntervalMessageCoordinator>()
                .As<IIntervalAction>().SingleInstance();
        }

        internal class Lazier<T> : Lazy<T> where T : class
        {
            public Lazier(IServiceProvider provider)
                : base(provider.GetRequiredService<T>)
            {
            }
        }
    }
}
