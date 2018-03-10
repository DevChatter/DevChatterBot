using System;
using System.Threading.Tasks;
using DevChatter.Bot.Core;
using DevChatter.Bot.Core.Events;
using TwitchLib;
using TwitchLib.Events.Client;
using TwitchLib.Events.Services.FollowerService;
using TwitchLib.Models.Client;
using TwitchLib.Services;

namespace DevChatter.Bot.Infra.Twitch
{
    public class TwitchChatClient : IChatClient
    {
        private readonly TwitchClient _twitchClient;
        private readonly TaskCompletionSource<bool> _connectionCompletionTask = new TaskCompletionSource<bool>();
        private bool _isReady = false;
        private readonly TwitchAPI _twitchApi;
        private FollowerService _followerService;

        public TwitchChatClient(TwitchClientSettings settings)
        {
            var credentials = new ConnectionCredentials(settings.TwitchUsername, settings.TwitchOAuth);
            _twitchClient = new TwitchClient(credentials, settings.TwitchChannel);
            _twitchApi = new TwitchAPI(settings.TwitchClientId);
            _followerService = new FollowerService(_twitchApi);
            _twitchClient.OnChatCommandReceived += ChatCommandReceived;
            _twitchClient.OnNewSubscriber += NewSubscriber;
            _followerService.OnNewFollowersDetected += NewFollower;
        }

        private void NewFollower(object sender, OnNewFollowersDetectedArgs e)
        {
            OnNewFollower?.Invoke(this, e.ToNewFollowerEventArgs());
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
        public event EventHandler<NewSubscriberEventArgs> OnNewSubscriber;
        public event EventHandler<NewFollowersEventArgs> OnNewFollower;
    }
}