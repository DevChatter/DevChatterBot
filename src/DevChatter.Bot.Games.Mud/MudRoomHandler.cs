using DevChatter.Bot.Core.Data.Model;
using DevChatter.Bot.Core.Events.Args;
using DevChatter.Bot.Core.Extensions;
using DevChatter.Bot.Core.Handlers;
using DevChatter.Bot.Core.Systems.Chat;

namespace DevChatter.Bot.Games.Mud
{
    public class MudRoomHandler : IBotHostedHandler
    {
        private readonly IChatClient _chatClient;
        private readonly MudGame _mudGame;

        public MudRoomHandler(IChatClient chatClient, MudGame mudGame)
        {
            _chatClient = chatClient;
            _mudGame = mudGame;
        }

        public void Connect()
        {
            _chatClient.OnMessageReceived += ChatClient_OnMessageReceived;
            _chatClient.OnCommandReceived += _chatClient_OnCommandReceived;
        }

        private void ChatClient_OnMessageReceived(
            object sender, MessageReceivedEventArgs e)
        {
            if (e.ChatUser.IsInThisRoleOrHigher(UserRole.Mod))
            {
            }
        }

        private void _chatClient_OnCommandReceived(
            object sender, CommandReceivedEventArgs e)
        {
            if (e.CommandWord.EqualsIns("JoinMud"))
            {
                _mudGame.AttemptToJoin(e.ChatUser);
            }
            if (e.CommandWord.EqualsIns("Look"))
            {
                _mudGame.Look(e.ChatUser);
            }
            if (e.CommandWord.EqualsIns("Move"))
            {
                _mudGame.Move(e.ChatUser, e.Arguments);
            }
        }

    }
}
