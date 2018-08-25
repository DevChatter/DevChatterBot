using System.Collections.Generic;
using DevChatter.Bot.Core.Data.Model;
using DevChatter.Bot.Core.Systems.Chat;

namespace DevChatter.Bot.Games.Mud.Actions
{
    public class Look : BaseMudAction
    {
        public Look(MudGame mudGame)
            : base(mudGame, nameof(Look))
        {
        }

        public override void Process(IMessageSender messageSender, ChatUser chatUser, IList<string> arguments)
        {
            _mudGame.Look(chatUser);
        }
    }
}
