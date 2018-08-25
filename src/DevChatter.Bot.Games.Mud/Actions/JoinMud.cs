using System.Collections.Generic;
using DevChatter.Bot.Core.Data.Model;
using DevChatter.Bot.Core.Systems.Chat;

namespace DevChatter.Bot.Games.Mud.Actions
{
    public class JoinMud : BaseMudAction
    {
        public JoinMud(MudGame mudGame)
            : base(mudGame, nameof(JoinMud))
        {
        }

        public override void Process(IMessageSender messageSender,
            ChatUser chatUser, IList<string> arguments)
        {
            _mudGame.AttemptToJoin(chatUser);
        }
    }
}
