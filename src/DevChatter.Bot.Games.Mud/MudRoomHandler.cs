using DevChatter.Bot.Core.Events.Args;
using DevChatter.Bot.Core.Handlers;
using DevChatter.Bot.Core.Systems.Chat;
using DevChatter.Bot.Games.Mud.Actions;
using System.Collections.Generic;
using System.Linq;

namespace DevChatter.Bot.Games.Mud
{
    public class MudRoomHandler : IBotHostedHandler
    {
        private readonly IList<IMudAction> _allActions;
        private readonly IChatClient _chatClient;

        public MudRoomHandler(IChatClient chatClient, IList<IMudAction> allActions)
        {
            _chatClient = chatClient;
            _allActions = allActions;
        }

        public void Connect()
        {
            _chatClient.OnMessageReceived += ChatClient_OnMessageReceived;
            _chatClient.OnCommandReceived += _chatClient_OnCommandReceived;
        }

        private void ChatClient_OnMessageReceived(
            object sender, MessageReceivedEventArgs e)
        {
        }

        private void _chatClient_OnCommandReceived(
            object sender, CommandReceivedEventArgs e)
        {
            var mudAction = _allActions.FirstOrDefault(a => a.CanExecute(e.CommandWord));
            mudAction?.Process(_chatClient, e.ChatUser, e.Arguments);
        }
    }
}
