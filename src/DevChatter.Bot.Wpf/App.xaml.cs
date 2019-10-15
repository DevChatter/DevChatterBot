using DevChatter.Bot.Web;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using System.ComponentModel;
using System.Windows;

namespace DevChatter.Bot.Wpf
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly BackgroundWorker worker = new BackgroundWorker();
        private IWebHost _webHost;

        public App()
        {
            _webHost = WebHost.CreateDefaultBuilder()
                .UseStartup<Startup>()
                .Build();
            _webHost.Start();

            //worker.DoWork += worker_DoWork;
            //worker.RunWorkerCompleted += worker_RunWorkerCompleted;
            //worker.RunWorkerAsync();
        }

        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            // run all background tasks here
        }

        private void worker_RunWorkerCompleted(object sender,
                                                   RunWorkerCompletedEventArgs e)
        {
            //update ui once worker complete his work
        }
    }
}
