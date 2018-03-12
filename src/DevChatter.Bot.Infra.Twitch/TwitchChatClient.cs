using System;
using System.Threading.Tasks;
using DevChatter.Bot.Core;
using DevChatter.Bot.Core.Events;
using TwitchLib;
using TwitchLib.Events.Client;
using TwitchLib.Models.Client;

namespace DevChatter.Bot.Infra.Twitch
{
    public class TwitchChatClient : IChatClient
    {
        private readonly TwitchClient _twitchClient;
        private readonly TaskCompletionSource<bool> _connectionCompletionTask = new TaskCompletionSource<bool>();
        private bool _isReady = false;

        public TwitchChatClient(TwitchClientSettings settings)
        {
            var credentials = new ConnectionCredentials(settings.TwitchUsername, settings.TwitchOAuth);
            _twitchClient = new TwitchClient(credentials, settings.TwitchChannel);
            _twitchClient.OnChatCommandReceived += ChatCommandReceived;
            _twitchClient.OnNewSubscriber += NewSubscriber;
        }

        private void NewSubscriber(object sender, OnNewSubscriberArgs e)
        {
            OnNewSubscriber?.Invoke(this, e.ToNewSubscriberEventArgs());
        }

        private void ChatCommandReceived(object sender, OnChatCommandReceivedArgs e)
        {
            OnCommandReceived?.Invoke(this, e.ToCommandReceivedEventArgs());
        }

        private void TwitchClientConnected(object sender, OnConnectedArgs onConnectedArgs)
        {
            _isReady = true;
            _connectionCompletionTask.SetResult(true);
            _twitchClient.SendMessage("Hello World! The bot has arrived!");
        }

        public async Task Connect()
        {
            _twitchClient.Connect();
            _twitchClient.OnConnected += TwitchClientConnected;

            await _connectionCompletionTask.Task;
        }

        public void SendMessage(string message)
        {
            if (_isReady)
            {
                _twitchClient.SendMessage(message);
            }
        }

        public event EventHandler<CommandReceivedEventArgs> OnCommandReceived;
        public event EventHandler<NewSubscriberEventArgs> OnNewSubscriber;
    }
}