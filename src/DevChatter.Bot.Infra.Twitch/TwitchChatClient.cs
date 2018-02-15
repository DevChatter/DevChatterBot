using DevChatter.Bot.Core;
using TwitchLib;
using TwitchLib.Events.Client;
using TwitchLib.Models.Client;

namespace DevChatter.Bot.Infra.Twitch
{
    public class TwitchChatClient : IChatClient
    {
        private bool _isReady = false;
        private readonly TwitchClient _twitchClient;

        public TwitchChatClient()
        {
            var connectionCredentials = Hidden.GetCredentials();
            _twitchClient = new TwitchClient(connectionCredentials, "devchatter");
            _twitchClient.Connect();
            _twitchClient.OnConnected += TwitchClientConnected;
        }

        private void TwitchClientConnected(object sender, OnConnectedArgs onConnectedArgs)
        {
            _isReady = true;
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