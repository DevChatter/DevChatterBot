using DevChatter.Bot.Core.Events.Args;
using DevChatter.Bot.Core.Systems.Chat;

namespace DevChatter.Bot.Games.Mud
{
    public class MudRoomHandler
    {
        public MudRoomHandler(IChatClient chatClient)
        {
            chatClient.OnMessageReceived += ChatClient_OnMessageReceived;
        }

        private void ChatClient_OnMessageReceived(object sender,
            MessageReceivedEventArgs e)
        {
            throw new System.NotImplementedException();
        }
    }
}
