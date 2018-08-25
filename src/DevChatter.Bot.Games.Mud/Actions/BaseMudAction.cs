using System.Collections.Generic;
using System.Linq;
using DevChatter.Bot.Core.Data.Model;
using DevChatter.Bot.Core.Extensions;
using DevChatter.Bot.Core.Systems.Chat;

namespace DevChatter.Bot.Games.Mud.Actions
{
    public abstract class BaseMudAction : IMudAction
    {
        protected readonly MudGame _mudGame;

        protected List<string> Verbs;

        protected BaseMudAction(MudGame mudGame, params string[] verbs)
        {
            _mudGame = mudGame;
            Verbs = new List<string>(verbs);
        }

        public virtual bool CanExecute(string actionText)
        {
            return Verbs.Any(v => actionText.EqualsIns(v));
        }

        public abstract void Process(IMessageSender messageSender, ChatUser chatUser, IList<string> arguments);
    }
}
