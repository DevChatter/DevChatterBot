using System;
using System.Linq;
using DevChatter.Bot.Core.Commands;
using DevChatter.Bot.Core.Data;
using DevChatter.Bot.Core.Data.Model;
using DevChatter.Bot.Core.Events.Args;
using DevChatter.Bot.Core.Systems.Chat;

namespace DevChatter.Bot.Core.Games.Heist
{
    public class HeistCommand : BaseCommand
    {
        private readonly HeistGame _heistGame;

        public HeistCommand(IRepository repository, HeistGame heistGame)
            : base(repository)
        {
            _heistGame = heistGame;
        }

        protected override bool HandleCommand(IChatClient chatClient, CommandReceivedEventArgs eventArgs)
        {
            if (eventArgs == null) throw new ArgumentNullException(nameof(eventArgs)); // How!?!?

            string roleRequest = eventArgs.Arguments?.ElementAtOrDefault(0);

            ChatUser chatUser = eventArgs.ChatUser;

            if (!_heistGame.IsGameRunning)
            {
                _heistGame.AttemptToCreateGame(chatClient, chatUser);
            }
            if (roleRequest == null)
            {
                JoinHeistRandom(chatClient, chatUser);
            }
            else if (Enum.TryParse(roleRequest, true, out HeistRoles role))
            {
                JoinHeistByRole(chatClient, chatUser, role);
            }
            else
            {
                chatClient.SendMessage("I don't know what role you wanted to be. Try again?");
            }
        }

        private void JoinHeistRandom(IChatClient chatClient, ChatUser chatUser)
        {
            JoinGameResult attemptToJoinHeist = _heistGame.AttemptToJoinHeist(chatUser.DisplayName, out HeistRoles role);

            chatClient.SendMessage(attemptToJoinHeist.Message);
        }

        private void JoinHeistByRole(IChatClient chatClient, ChatUser chatUser, HeistRoles role)
        {
            JoinGameResult attemptToJoinHeist = _heistGame.AttemptToJoinHeist(chatUser.DisplayName, role);
            chatClient.SendMessage(attemptToJoinHeist.Message);
        }
    }
}
