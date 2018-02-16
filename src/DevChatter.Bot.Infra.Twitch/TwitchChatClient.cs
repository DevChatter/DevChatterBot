using System.Threading;
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

        public TwitchChatClient()
        {
            ConnectionCredentials connectionCredentials = Hidden.GetCredentials();
            _twitchClient = new TwitchClient(connectionCredentials, "devchatter");
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