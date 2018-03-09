using System;
using System.Threading.Tasks;
using DevChatter.Bot.Core;
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
        }

        private void ChatCommandReceived(object sender, OnChatCommandReceivedArgs e)
        {
            OnCommandReceived?.Invoke(this, e.ToCommandReceivedEventArgs());
        }

        private void TwitchClientConnected(object sender, OnConnectedArgs onConnectedArgs)
        {
            _isReady = true;
            _connectionCompletionTask.SetResult(true);

            _twitchClient.OnUserJoined += TwitchClientOnOnUserJoined;
        }

        private void TwitchClientOnOnUserJoined(object sender, OnUserJoinedArgs onUserJoinedArgs)
        {
            // Hey! Welcome back, it's your #th day here!
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
    }
}