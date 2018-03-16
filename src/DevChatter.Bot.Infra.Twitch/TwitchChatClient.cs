using System;
using System.Threading.Tasks;
using DevChatter.Bot.Core.ChatSystems;
using DevChatter.Bot.Core.Events;
using TwitchLib;
using TwitchLib.Events.Client;
using TwitchLib.Models.Client;

namespace DevChatter.Bot.Infra.Twitch
{
    public class TwitchChatClient : IChatClient
    {
        private readonly TwitchClient _twitchClient;
        private TaskCompletionSource<bool> _connectionCompletionTask = new TaskCompletionSource<bool>();
        private TaskCompletionSource<bool> _disconnectionCompletionTask = new TaskCompletionSource<bool>();
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

        public async Task Connect()
        {
            _twitchClient.OnConnected += TwitchClientConnected;
            _twitchClient.Connect();

            await _connectionCompletionTask.Task;
        }

        private void TwitchClientConnected(object sender, OnConnectedArgs onConnectedArgs)
        {
            _twitchClient.OnConnected -= TwitchClientConnected;

            _isReady = true;
            _connectionCompletionTask.SetResult(true);
            _disconnectionCompletionTask = new TaskCompletionSource<bool>();
            _twitchClient.SendMessage("Hello World! The bot has arrived!");
        }

        public async Task Disconnect()
        {
            _twitchClient.OnDisconnected += TwitchClientOnOnDisconnected;
            _twitchClient.Disconnect();

            await _connectionCompletionTask.Task;
        }

        private void TwitchClientOnOnDisconnected(object sender, OnDisconnectedArgs onDisconnectedArgs)
        {
            _twitchClient.OnDisconnected -= TwitchClientOnOnDisconnected;
            _isReady = false;

            _disconnectionCompletionTask.SetResult(true);
            _connectionCompletionTask = new TaskCompletionSource<bool>();
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