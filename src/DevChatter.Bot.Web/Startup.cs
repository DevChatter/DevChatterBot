using System;
using System.Collections.Generic;
using System.Linq;
using DevChatter.Bot.Core;
using DevChatter.Bot.Core.Automation;
using DevChatter.Bot.Core.Commands;
using DevChatter.Bot.Core.Commands.Trackers;
using DevChatter.Bot.Core.Data;
using DevChatter.Bot.Core.Data.Specifications;
using DevChatter.Bot.Core.Events;
using DevChatter.Bot.Core.Games.Hangman;
using DevChatter.Bot.Core.Games.Heist;
using DevChatter.Bot.Core.Games.Quiz;
using DevChatter.Bot.Core.Games.RockPaperScissors;
using DevChatter.Bot.Core.Games.Roulette;
using DevChatter.Bot.Core.Systems.Chat;
using DevChatter.Bot.Core.Systems.Streaming;
using DevChatter.Bot.Core.Util;
using DevChatter.Bot.Infra.Ef;
using DevChatter.Bot.Infra.Twitch;
using DevChatter.Bot.Infra.Twitch.Events;
using Hangfire;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TwitchLib.Api;
using TwitchLib.Api.Interfaces;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace DevChatter.Bot.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.Configure<CommandHandlerSettings>(Configuration.GetSection("CommandHandlerSettings"));
            services.Configure<TwitchClientSettings>(Configuration.GetSection("TwitchClientSettings"));

            var fullConfig = Configuration.Get<BotConfiguration>();

            services.AddSingleton(fullConfig.TwitchClientSettings);
            services.AddSingleton(fullConfig.CommandHandlerSettings);

            services.Configure<BotConfiguration>(Configuration);

            services.AddSingleton<ILoggerFactory,LoggerFactory>();

            IRepository repository = SetUpDatabase.SetUpRepository(fullConfig.DatabaseConnectionString);

            services.AddSingleton(repository);

            services.AddSingleton<IStreamingPlatform, StreamingPlatform>();
            services.AddSingleton<IClock, SystemClock>();

            services.AddSingleton(typeof(ILogger<>), typeof(Logger<>));
            services.AddSingleton(typeof(ILoggerAdapter<>), typeof(LoggerAdapter<>));

            services.AddSingleton<IChatUserCollection, ChatUserCollection>();
            services.AddSingleton<ICurrencyGenerator, CurrencyGenerator>();
            services.AddSingleton<IAutomatedActionSystem, HangfireAutomationSystem>();
            services.AddSingleton<ISettingsFactory, SettingsFactory>();
            services.AddSingleton<IBotCommand, RouletteCommand>();
            services.AddSingleton<RockPaperScissorsGame>();
            services.AddSingleton<IBotCommand, RockPaperScissorsCommand>();
            services.AddSingleton<IBotCommand, HeistCommand>();
            services.AddSingleton<HeistGame>();
            services.AddSingleton<IBotCommand, QuizCommand>();
            services.AddSingleton<QuizGame>();

            services.AddSingleton<HangmanGame>();
            services.AddSingleton<IBotCommand, HangmanCommand>();
            services.AddSingleton<IBotCommand, TopCommand>();
            services.AddSingleton<IBotCommand, UptimeCommand>();
            services.AddSingleton<IBotCommand, TaxCommand>();
            services.AddSingleton<IBotCommand, GiveCommand>();
            services.AddSingleton<IBotCommand, CoinsCommand>();
            services.AddSingleton<IBotCommand, BonusCommand>();
            services.AddSingleton<IBotCommand, StreamsCommand>();
            services.AddSingleton<IBotCommand, ShoutOutCommand>();
            services.AddSingleton<IBotCommand, QuoteCommand>();
            services.AddSingleton<IBotCommand, AliasCommand>();
            services.AddSingleton<IBotCommand, ScheduleCommand>();
            services.AddSingleton(typeof(IList<>), typeof(List<>));
            services.AddTransient(typeof(Lazy<>), typeof(Lazier<>));

            SetUpSimpleCommands(services, repository);

            services.AddSingleton<IBotCommand, HelpCommand>(); // TODO: make these work
            services.AddSingleton<IBotCommand, CommandsCommand>(); // TODO: make these work

            services.AddSingleton(provider => new CommandList(provider.GetServices<IBotCommand>().ToList()));

            //builder.Register(ctx => new HelpCommand(ctx.Resolve<IRepository>()))
            //    .OnActivated(e => e.Instance.AllCommands = e.Context.Resolve<CommandList>())
            //    .AsImplementedInterfaces();
            //builder.Register(ctx => new CommandsCommand(ctx.Resolve<IRepository>()))
            //    .OnActivated(e => e.Instance.AllCommands = e.Context.Resolve<CommandList>())
            //    .AsImplementedInterfaces();

            //builder.Register(ctx => new CommandList(ctx.Resolve<IList<IBotCommand>>()))
            //    .AsSelf()
            //    .SingleInstance();

            services.AddSingleton<ICommandUsageTracker, CommandCooldownTracker>();
            services.AddSingleton<ICommandHandler, CommandHandler>();
            services.AddSingleton<SubscriberHandler>();
            services.AddSingleton<IFollowableSystem, FollowableSystem>();
            services.AddSingleton<IFollowerService, TwitchFollowerService>();

            var api = new TwitchAPI();
            api.Settings.ClientId = fullConfig.TwitchClientSettings.TwitchClientId;
            api.Settings.AccessToken = fullConfig.TwitchClientSettings.TwitchChannelOAuth;
            services.AddSingleton<ITwitchAPI>(api);

            services.AddSingleton<IChatClient, TwitchChatClient>();

            services.AddSingleton<IStreamingInfoService, TwitchStreamingInfoService>();

            services.AddSingleton<BotMain>();

            services.AddSingleton<IHostedService, DevChatterBotBackgroundWorker>();

            services.AddDbContext<AppDataContext>(ServiceLifetime.Transient);

            services.AddHangfire(cfg => cfg.UseSqlServerStorage(fullConfig.DatabaseConnectionString));

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        private void SetUpSimpleCommands(IServiceCollection services, IRepository repository)
        {
            List<SimpleCommand> simpleCommands = repository.List(CommandPolicy.All());

            foreach (SimpleCommand simpleCommand in simpleCommands)
            {
                services.AddSingleton<IBotCommand>(simpleCommand);
            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseHangfireDashboard();
            app.UseHangfireServer();

            app.UseMvc();
        }
    }

    internal class Lazier<T> : Lazy<T> where T : class
    {
        public Lazier(IServiceProvider provider)
            : base(provider.GetRequiredService<T>)
        {
        }
    }
}
