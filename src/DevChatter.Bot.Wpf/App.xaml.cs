using DevChatter.Bot.Core.Extensions;
using DevChatter.Bot.Web;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Windows;

namespace DevChatter.Bot.Wpf
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly IHost _webHost;

        public App()
        {
            _webHost = Host.CreateDefaultBuilder()
                .ConfigureWebHostDefaults(
                    hostBuilder =>
                    {
                        hostBuilder.UseStartup<Startup>();
                    })
                .ConfigureAppConfiguration(configBuilder => configBuilder
                    .AddJsonFile("appsettings.json",
                        optional: false,
                        reloadOnChange: true)
                    .AddEnvironmentVariables()
                    .AddUserSecrets<App>())
                .Build();

            _webHost.Start();

            Exit += OnExit;
        }

        private void OnExit(object sender, ExitEventArgs e)
        {
            _webHost.Services.GetService<IHostApplicationLifetime>().StopApplication();
            _webHost.StopAsync().RunInBackgroundSafely(HandleException);
        }

        private void HandleException(Exception obj)
        {
            // TODO: Do something with this.
        }
    }
}
