using System.Collections.Generic;
using DevChatter.Bot.Core.Data.Model;
using DevChatter.Bot.Core.Systems.Chat;

namespace DevChatter.Bot.Games.Mud.Actions
{
    public class Move : BaseMudAction
    {
        public Move(MudGame mudGame)
            : base(mudGame, nameof(Move))
        {
        }

        public override void Process(IMessageSender messageSender, ChatUser chatUser, IList<string> arguments)
        {
            _mudGame.Move(chatUser, arguments);
        }
    }
}
