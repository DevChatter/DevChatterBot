using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using DevChatter.Bot.Core.Systems.Chat;

namespace DevChatter.Bot.Infra.Twitch
{
    public class TwitchKnownBotService : IKnownBotService
    {
        private const string API_ENDPOINT = "https://api.twitchbots.info/v1/";

        private readonly HttpClient _client;

        public TwitchKnownBotService()
        {
            _client = new HttpClient();
        }

        public async Task<bool> IsKnownBot(string username)
        {
            HttpResponseMessage response = await _client.GetAsync($"{API_ENDPOINT}bot/{username}").ConfigureAwait(false);

            switch (response.StatusCode)
            {
                case HttpStatusCode.NotFound:
                    return false;

                case HttpStatusCode.OK:
                    return true;

                default:
                    throw new HttpRequestException(
                        $"Unable to fetch known bot. Service returned {(int) response.StatusCode} {response.StatusCode}");
            }
        }
    }
}
