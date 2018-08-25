using System.Collections.Generic;
using System.Linq;
using DevChatter.Bot.Core.Data.Model;
using DevChatter.Bot.Core.Systems.Chat;

namespace DevChatter.Bot.Games.Mud.Actions
{
    public class Take : BaseMudAction
    {
        public Take(MudGame mudGame)
            : base(mudGame, nameof(Take))
        {
        }

        public override void Process(IMessageSender messageSender, ChatUser chatUser, IList<string> arguments)
        {
            string itemToTake = arguments.FirstOrDefault();
            if (itemToTake != null)
            {
                _mudGame.Take(chatUser, itemToTake);
            }
            else
            {
                messageSender.SendDirectMessage(chatUser.DisplayName,
                    "You have to specify an item to take.");
            }
        }
    }
}
