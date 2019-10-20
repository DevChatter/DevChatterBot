using Autofac;
using Autofac.Extensions.DependencyInjection;
using DevChatter.Bot.Core;
using DevChatter.Bot.Core.Data;
using DevChatter.Bot.Core.Events;
using DevChatter.Bot.Infra.Ef;
using DevChatter.Bot.Infra.Twitch;
using DevChatter.Bot.Infra.Web.Hubs;
using DevChatter.Bot.Modules.WastefulGame.Hubs;
using DevChatter.Bot.Modules.WastefulGame.Startup;
using DevChatter.Bot.Web.Modules;
using DevChatter.Bot.Web.Setup;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DevChatter.Bot.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public IRepository Repository { get; set; }

        public ILifetimeScope AutofacContainer { get; private set; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.Configure<CommandHandlerSettings>(Configuration.GetSection("CommandHandlerSettings"));
            services.Configure<TwitchClientSettings>(Configuration.GetSection("TwitchClientSettings"));
            services.Configure<GoogleCloudSettings>(Configuration.GetSection("GoogleCloudSettings"));

            services.AddDbContext<AppDataContext>(ServiceLifetime.Transient);

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

            services.AddSignalR();

            var fullConfig = Configuration.Get<BotConfiguration>();

            Repository = SetUpDatabase.SetUpRepository(fullConfig);
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            var fullConfig = Configuration.Get<BotConfiguration>();

            builder.RegisterModule(new ConfigurationModule(fullConfig));

            builder.RegisterModule<DefaultAutofacModule>();

            builder.RegisterModule(new CommandsModule(Repository));

            builder.RegisterModule<WastefulAutofacModule>();

            builder.RegisterModule<CoreRegistrationModule>();

            builder.RegisterModule(new TwitchModule(fullConfig.TwitchClientSettings));

            builder.RegisterModule<GamesModule>();

            builder.RegisterModule<CurrencyModule>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
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

            app.UseRouting();

            AutofacContainer = app.ApplicationServices.GetAutofacRoot();

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<BotHub>("/BotHub");
                endpoints.MapHub<VotingHub>("/VotingHub");
                endpoints.MapHub<HangmanHub>("/HangmanHub");
                endpoints.MapHub<WastefulHub>("/WastefulHub");

                endpoints.MapControllers();
                endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
