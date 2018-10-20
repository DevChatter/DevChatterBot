using DevChatter.Bot.Core.Data.Model;
using DevChatter.Bot.Core.Events.Args;
using DevChatter.Bot.Core.Extensions;
using DevChatter.Bot.Core.Systems.Chat;
using DevChatter.Bot.Infra.Twitch.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DevChatter.Bot.Core.Util;
using TwitchLib.Api.Core.Models.Undocumented.Chatters;
using TwitchLib.Api.Interfaces;
using TwitchLib.Client;
using TwitchLib.Client.Events;
using TwitchLib.Client.Extensions;
using TwitchLib.Client.Interfaces;
using TwitchLib.Client.Models;
using TwitchLib.Communication.Events;

namespace DevChatter.Bot.Infra.Twitch
{
    public class TwitchChatClient : IChatClient
    {
        private readonly TwitchClientSettings _settings;
        private readonly ITwitchAPI _twitchApi;
        private readonly ILoggerAdapter<TwitchChatClient> _logger;
        private readonly ITwitchClient _twitchClient;
        private TaskCompletionSource<bool> _connectionCompletionTask = new TaskCompletionSource<bool>();
        private TaskCompletionSource<bool> _disconnectionCompletionTask = new TaskCompletionSource<bool>();
        private bool _isReady = false;

        public TwitchChatClient(TwitchClientSettings settings,
            ITwitchAPI twitchApi, ILoggerAdapter<TwitchChatClient> logger)
        {
            _settings = settings;
            _twitchApi = twitchApi;
            _logger = logger;
            var credentials = new ConnectionCredentials(settings.TwitchUsername, settings.TwitchBotOAuth);
            _twitchClient = new TwitchClient();
            _twitchClient.Initialize(credentials, channel:settings.TwitchChannel);
            _twitchClient.OnChatCommandReceived += ChatCommandReceived;
            _twitchClient.OnNewSubscriber += NewSubscriber;
            _twitchClient.OnUserJoined += TwitchClientOnOnUserJoined;
            _twitchClient.OnUserLeft += TwitchClientOnOnUserLeft;
            _twitchClient.OnWhisperReceived += TwitchWhisperReceived;
        }

        private void TwitchWhisperReceived(object sender, OnWhisperReceivedArgs e)
        {
            OnWhisperReceived?.Invoke(this, e.ToWhisperReceivedEventArgs());
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
            _logger.LogInformation($"{nameof(TwitchChatClient)} connected");
            _twitchClient.OnConnected -= TwitchClientConnected;

            _isReady = true;
            _connectionCompletionTask.SetResult(true);
            _disconnectionCompletionTask = new TaskCompletionSource<bool>();

            JoinChannelRoom(_settings.TwitchRoomId);
        }

        private void JoinChannelRoom(string roomId)
        {
            try
            {
                _twitchClient.JoinRoom(_settings.TwitchChannelId, roomId);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task Disconnect()
        {
            _twitchClient.OnDisconnected += TwitchClientOnOnDisconnected;
            _twitchClient.Disconnect();

            await _disconnectionCompletionTask.Task;
        }

        private void TwitchClientOnOnDisconnected(object sender, OnDisconnectedEventArgs e)
        {
            _logger.LogInformation($"{nameof(TwitchChatClient)} disconnected");
            _twitchClient.OnDisconnected -= TwitchClientOnOnDisconnected;
            _isReady = false;

            _disconnectionCompletionTask.SetResult(true);
            _connectionCompletionTask = new TaskCompletionSource<bool>();
        }

        public void SendMessage(string message)
        {
            if (_isReady)
            {
                _twitchClient.SendMessage(_settings.TwitchChannel, message);
            }
        }

        public void Timeout(string username, TimeSpan duration, string reason)
        {
            _twitchClient.TimeoutUser(_settings.TwitchChannel, username, duration, reason);
        }

        public IList<ChatUser> GetAllChatters()
        {
            _logger.LogInformation("Getting all Twitch chatters");
            List<ChatterFormatted> chatters = _twitchApi.Undocumented.GetChattersAsync(_settings.TwitchChannel)
                .TryGetResult(10).Result;
            var chatUsers = chatters.Select(x => x.ToChatUser()).ToList();
            _logger.LogInformation("Returning all Twitch chatters");
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

        public event EventHandler<WhisperReceivedEventArgs> OnWhisperReceived;
    }
}
