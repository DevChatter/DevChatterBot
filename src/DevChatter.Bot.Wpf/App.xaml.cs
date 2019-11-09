using Autofac.Extensions.DependencyInjection;
using DevChatter.Bot.Web;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
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
            _webHost = CreateWebHostBuilder().Build();
        }

        public static IHostBuilder CreateWebHostBuilder()
        {
            return Host.CreateDefaultBuilder()
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureWebHostDefaults(
                    webBuilder =>
                    {
                        webBuilder.UseStartup<Startup>();
                    })
                .ConfigureAppConfiguration(configBuilder =>
                {
                    configBuilder
                        .AddJsonFile("appsettings.json",
                            optional: false,
                            reloadOnChange: true)
                        .AddEnvironmentVariables()
                        .AddUserSecrets<App>();
                });
        }

        private void App_OnStartup(object sender, StartupEventArgs e)
        {
            _webHost.Start();

            _webHost.Services.GetRequiredService<MainWindow>().Show();
        }

        private async void App_OnExit(object sender, ExitEventArgs e)
        {
            _webHost.Services.GetService<IHostApplicationLifetime>().StopApplication();
            await _webHost.StopAsync();
            _webHost.Dispose();
        }
    }
}
