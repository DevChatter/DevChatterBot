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
            var credentials = new ConnectionCredentials(settings.Username, settings.OAuth);
            _twitchClient = new TwitchClient(credentials, settings.Channel);
        }

        private void TwitchClientConnected(object sender, OnConnectedArgs onConnectedArgs)
        {
            _isReady = true;
            _connectionCompletionTask.SetResult(true);
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
    }
}