using DevChatter.Bot.Core.Data.Model;
using DevChatter.Bot.Core.Events.Args;
using DevChatter.Bot.Core.Systems.Chat;
using DevChatter.Bot.Core.Extensions;
using DevChatter.Bot.Infra.Twitch.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TwitchLib;
using TwitchLib.Events.Client;
using TwitchLib.Models.API.Undocumented.Chatters;
using TwitchLib.Models.Client;

namespace DevChatter.Bot.Infra.Twitch
{
    public class TwitchChatClient : IChatClient
    {
        private readonly TwitchClientSettings _settings;
        private readonly ITwitchAPI _twitchApi;
        private readonly ITwitchClient _twitchClient;
        private TaskCompletionSource<bool> _connectionCompletionTask = new TaskCompletionSource<bool>();
        private TaskCompletionSource<bool> _disconnectionCompletionTask = new TaskCompletionSource<bool>();
        private bool _isReady = false;

        public TwitchChatClient(TwitchClientSettings settings, ITwitchAPI twitchApi)
        {
            _settings = settings;
            _twitchApi = twitchApi;
            var credentials = new ConnectionCredentials(settings.TwitchUsername, settings.TwitchOAuth);
            _twitchClient = new TwitchClient(credentials, settings.TwitchChannel);
            _twitchClient.OnChatCommandReceived += ChatCommandReceived;
            _twitchClient.OnNewSubscriber += NewSubscriber;
            _twitchClient.OnUserJoined += TwitchClientOnOnUserJoined;
            _twitchClient.OnUserLeft += TwitchClientOnOnUserLeft;
        }

        private void TwitchClientOnOnUserLeft(object sender, OnUserLeftArgs onUserLeftArgs)
        {
            OnUserLeft?.Invoke(this, onUserLeftArgs.ToUserStatusEventArgs());
        }

        private void TwitchClientOnOnUserJoined(object sender, OnUserJoinedArgs onUserJoinedArgs)
        {
            OnUserNoticed?.Invoke(this, onUserJoinedArgs.ToUserStatusEventArgs());
        }

        private void NewSubscriber(object sender, OnNewSubscriberArgs e)
        {
            OnNewSubscriber?.Invoke(this, e.ToNewSubscriberEventArgs());
        }

        private void ChatCommandReceived(object sender, OnChatCommandReceivedArgs e)
        {
            OnCommandReceived?.Invoke(this, e.ToCommandReceivedEventArgs());
            OnUserNoticed?.Invoke(this, e.ToUserStatusEventArgs());
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

            await _disconnectionCompletionTask.Task;
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

        public IList<ChatUser> GetAllChatters()
        {
            List<ChatterFormatted> chatters = _twitchApi.Undocumented.GetChattersAsync(_settings.TwitchChannel).TryGetResult().Result;
            var chatUsers = chatters.Select(x => x.ToChatUser()).ToList();
            return chatUsers;
        }

        public void SendDirectMessage(string username, string message)
        {
            if (_isReady)
            {
                _twitchClient.SendWhisper(username, message);
            }
        }

        public event EventHandler<CommandReceivedEventArgs> OnCommandReceived;
        public event EventHandler<NewSubscriberEventArgs> OnNewSubscriber;
        public event EventHandler<UserStatusEventArgs> OnUserNoticed;
        public event EventHandler<UserStatusEventArgs> OnUserLeft;
    }
}
