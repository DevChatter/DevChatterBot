using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DevChatter.Bot.Core.Data.Model;
using DevChatter.Bot.Core.Events;
using DevChatter.Bot.Core.Systems.Chat;
using DevChatter.Bot.Core.Util;
using DevChatter.Bot.Infra.Discord.Extensions;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace DevChatter.Bot.Infra.Discord
{
    public class DiscordChatClient : IChatClient
    {
        private readonly DiscordClientSettings _settings;
        private readonly DiscordSocketClient _discordClient;
        private TaskCompletionSource<bool> _connectionCompletionTask = new TaskCompletionSource<bool>();
        private TaskCompletionSource<bool> _disconnectionCompletionTask = new TaskCompletionSource<bool>();
        private SocketGuild _guild;
        private readonly List<ulong> _guildChannels = new List<ulong>();
        private ISocketMessageChannel _textChannel;
        private bool _isReady;

        public DiscordChatClient(DiscordClientSettings settings)
        {
            _settings = settings;
            _discordClient = new DiscordSocketClient();
            _discordClient.MessageReceived += DiscordClientMessageReceived;
            _discordClient.GuildAvailable += DiscordClientGuildAvailable;
            _discordClient.GuildUnavailable += DiscordClientGuildUnavailable;
            _discordClient.ChannelCreated += DiscordClientChannelCreated;
            _discordClient.ChannelDestroyed += DiscordClientChannelDestroyed;
            _discordClient.UserJoined += DiscordClientUserJoined;
            _discordClient.UserLeft += DiscordClientUserLeft;
        }

        private async Task DiscordClientGuildAvailable(SocketGuild guild)
        {
            _guild = guild;
            _guildChannels.AddRange(_guild.Channels.Select(channel => channel.Id));
            _textChannel = _guild.Channels.FirstOrDefault(channel => channel.Id == _settings.DiscordTextChannelId) as ISocketMessageChannel;
            _isReady = true;
        }

        private async Task DiscordClientGuildUnavailable(SocketGuild guild)
        {
            _guild = null;
            _guildChannels.Clear();
            _isReady = false;
        }

        private async Task DiscordClientChannelCreated(SocketChannel newChannel)
        {
            _guildChannels.Add(newChannel.Id);
        }

        private async Task DiscordClientChannelDestroyed(SocketChannel oldChannel)
        {
            _guildChannels.Remove(oldChannel.Id);
        }

        private async Task DiscordClientMessageReceived(SocketMessage arg)
        {
            var message = arg as SocketUserMessage;
            if (message == null)
            {
                return;
            }

            int commandStartIndex = 0;
            if (message.HasCharPrefix(_settings.CommandPrefix, ref commandStartIndex))
            {
                if (_guildChannels.Contains(message.Channel.Id))
                {
                    if (arg.Author is IGuildUser guildUser)
                    {
                        GuildCommandReceived(guildUser, commandStartIndex, arg.Content);
                    }
                }
                else
                {
                    DirectCommandReceieved(arg.Author, commandStartIndex, arg.Content);
                }
            }
        }

        private void GuildCommandReceived(IGuildUser user, int commandStartIndex, string message)
        {
            var commandInfo = CommandParser.Parse(message, commandStartIndex);
            if (string.IsNullOrWhiteSpace(commandInfo.commandWord)) return;

            RaiseOnCommandReceived(user, commandInfo.commandWord, commandInfo.arguments);
        }

        private void DirectCommandReceieved(IUser user, int commandStartIndex, string message)
        {
            var commandInfo = CommandParser.Parse(message, commandStartIndex);
            if (string.IsNullOrWhiteSpace(commandInfo.commandWord)) return;

            // TODO: Do we want to handle direct message commands?
        }

        public async Task Connect()
        {
            _discordClient.Connected += DiscordClientConnected;
            await _discordClient.LoginAsync(TokenType.Bot, _settings.DiscordToken).ConfigureAwait(false);
            await _discordClient.StartAsync().ConfigureAwait(false);

            await _connectionCompletionTask.Task;
        }

        private async Task DiscordClientConnected()
        {
            _discordClient.Connected -= DiscordClientConnected;

            _connectionCompletionTask?.SetResult(true);
            _disconnectionCompletionTask = new TaskCompletionSource<bool>();
        }

        public async Task Disconnect()
        {
            _discordClient.Disconnected += DiscordClientDisconnected;
            await _discordClient.LogoutAsync().ConfigureAwait(false);
            await _discordClient.StopAsync().ConfigureAwait(false);

            await _disconnectionCompletionTask.Task;
        }

        private async Task DiscordClientDisconnected(Exception arg)
        {
            _discordClient.Disconnected -= DiscordClientDisconnected;

            _disconnectionCompletionTask.SetResult(true);
            _connectionCompletionTask = new TaskCompletionSource<bool>();
        }

        private async Task DiscordClientUserJoined(SocketGuildUser arg)
        {
            RaiseOnUserNoticed(arg);
        }

        private async Task DiscordClientUserLeft(SocketGuildUser arg)
        {
            RaiseOnUserLeft(arg);
        }

        public IList<ChatUser> GetAllChatters()
        {
            if(!_isReady)
                return new List<ChatUser>();

            var chatUsers = _guild.Users.Select(user => user.ToChatUser(_settings)).ToList();
            return chatUsers;
        }

        public void SendMessage(string message)
        {
            if (!_isReady)
            {
                return;
            }

            _textChannel.SendMessageAsync($"`{message}`").Wait();
        }

        private void RaiseOnCommandReceived(IGuildUser user, string commandWord, List<string> arguments)
        {
            var eventArgs = new CommandReceivedEventArgs
            {
                CommandWord = commandWord,
                Arguments = arguments ?? new List<string>(),
                ChatUser = user.ToChatUser(_settings)
            };

            OnCommandReceived?.Invoke(this, eventArgs);
        }

        private void RaiseOnUserNoticed(SocketGuildUser user)
        {
            var eventArgs = new UserStatusEventArgs
            {
                DisplayName = user.Username,
                Role = user.ToUserRole(_settings)
            };

            OnUserNoticed?.Invoke(this, eventArgs);
        }

        private void RaiseOnUserLeft(SocketGuildUser user)
        {
            var eventArgs = new UserStatusEventArgs
            {
                DisplayName = user.Username,
                Role = user.ToUserRole(_settings)
            };

            OnUserLeft?.Invoke(this, eventArgs);
        }

        public event EventHandler<CommandReceivedEventArgs> OnCommandReceived;
        public event EventHandler<NewSubscriberEventArgs> OnNewSubscriber;
        public event EventHandler<UserStatusEventArgs> OnUserNoticed;
        public event EventHandler<UserStatusEventArgs> OnUserLeft;
    }
}
