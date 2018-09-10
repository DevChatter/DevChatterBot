using DevChatter.Bot.Core.Data.Model;
using DevChatter.Bot.Core.Events.Args;
using DevChatter.Bot.Core.Systems.Chat;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using DevChatter.Bot.Core.Util;
using System.Linq;

namespace DevChatter.Bot.Infra.Discord
{
    public class DiscordChatClient : IChatClient
    {
        private readonly DiscordClientSettings _settings;
        private readonly ILoggerAdapter<DiscordChatClient> _logger;
        private readonly DiscordSocketClient _discordClient;
        private TaskCompletionSource<bool> _connectionCompletionTask = new TaskCompletionSource<bool>();
        private TaskCompletionSource<bool> _disconnectionCompletionTask = new TaskCompletionSource<bool>();
        private bool _isReady;
        private SocketTextChannel _channel;
        private SocketTextChannel _channel1;

        public DiscordChatClient(DiscordClientSettings settings, ILoggerAdapter<DiscordChatClient> logger)
        {
            _settings = settings;
            _logger = logger;
            _discordClient = new DiscordSocketClient();

            _discordClient.MessageReceived += ChatCommandReceived;
        }

        public event EventHandler<CommandReceivedEventArgs> OnCommandReceived;
        public event EventHandler<UserStatusEventArgs> OnUserNoticed;
        public event EventHandler<UserStatusEventArgs> OnUserLeft;

        public async Task Connect()
        {
            _discordClient.Connected += DiscordClientConnected;


            await _discordClient.LoginAsync(TokenType.Bot, _settings.DiscordToken);
            await _discordClient.StartAsync();

            // TODO: Figure out how to check for a _real_ ready state.
            await Task.Delay(5000);
            await _connectionCompletionTask.Task;
            _channel = _discordClient.GetChannel(377567274581753861) as SocketTextChannel;
        }

        private Task DiscordClientConnected()
        {
            _logger.LogInformation($"{nameof(DiscordChatClient)} connected");
            _discordClient.Connected -= DiscordClientConnected;

            var state = _discordClient.ConnectionState;

            _isReady = true;
            _connectionCompletionTask.SetResult(true);
            _disconnectionCompletionTask = new TaskCompletionSource<bool>();

            return Task.CompletedTask;
        }

        public async Task Disconnect()
        {
            _discordClient.Disconnected += DiscordClientDisconnected;
            await _discordClient.StopAsync();

            await _disconnectionCompletionTask.Task;
        }

        private Task DiscordClientDisconnected(Exception arg)
        {
            _logger.LogInformation($"{nameof(DiscordChatClient)} disconnected");
            _discordClient.Disconnected -= DiscordClientDisconnected;
            _isReady = false;

            _disconnectionCompletionTask.SetResult(true);
            _connectionCompletionTask = new TaskCompletionSource<bool>();

            return Task.CompletedTask;
        }

        public IList<ChatUser> GetAllChatters()
        {
            var userList = new List<ChatUser>();

            if (_isReady)
            {
                var channel = (SocketChannel)Channel;

                foreach (var user in channel.Users)
                {
                    userList.Add(new ChatUser
                    {
                        UserId = user.Id.ToString(),
                        DisplayName = user.Username,
                    });
                }
            }
            return userList;
        }

        public void SendDirectMessage(string username, string message)
        {
            throw new NotImplementedException();
        }

        public void SendMessage(string message)
        {
            if (_isReady)
            {
                Channel?.SendMessageAsync(message);
            }
        }

        // TODO: Check how to timeout someone at discord
        public void Timeout(string username, TimeSpan duration, string reason)
        {
            throw new NotImplementedException();
        }

        SocketTextChannel Channel {
            get => _channel ?? _discordClient.GetChannel(_settings.DiscordChannelId) as SocketTextChannel;
            set => _channel1 = value;
        }

        // TODO: Add Mapping to Config, maybe?
        private UserRole DetermineUserRole(ulong id)
        {
            var roles = Channel.GetUser(id).Roles;
            if (roles.Where(x => x.Permissions.Administrator == true).Count() > 0)
            {
                return UserRole.Streamer;
            }
            return UserRole.Everyone;
        }

        private async Task ChatCommandReceived(SocketMessage command)
        {
            var commandText = command.Content.ToString();
            if (commandText.Substring(0,1) == "!")
            {
                var arguments = commandText.Split(" ").ToList();
                commandText = arguments[0].Substring(1);
                arguments.RemoveAt(0);
                var eventArgs = new CommandReceivedEventArgs
                {
                    CommandWord = commandText,
                    ChatUser = new ChatUser
                    {
                        DisplayName = command.Author.Username,
                        UserId = command.Author.Id.ToString(),
                        Role = DetermineUserRole(command.Author.Id)
                    },
                    Arguments = arguments
                };


                OnCommandReceived?.Invoke(this, eventArgs);
            }

            await Task.CompletedTask;
        }
    }
}
