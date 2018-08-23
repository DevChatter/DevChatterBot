using System.Collections.Generic;
using System.Linq;
using DevChatter.Bot.Core.Data.Model;
using DevChatter.Bot.Core.Extensions;
using DevChatter.Bot.Core.Games;
using DevChatter.Bot.Core.Systems.Chat;
using DevChatter.Bot.Games.Mud.Data;
using DevChatter.Bot.Games.Mud.Data.Model;

namespace DevChatter.Bot.Games.Mud
{
    public class MudGame : IGame
    {
        private readonly List<string> _joinedPlayers = new List<string>();

        private readonly IMessageSender _messageSender;
        private readonly Room _dungeonEntrance;

        public MudGame(IMessageSender messageSender)
        {
            _messageSender = messageSender;
            _dungeonEntrance = FakeData.GetDungeon();
        }

        public bool IsRunning { get; } = true;

        public void AttemptToJoin(ChatUser chatUser)
        {
            if (IsAlreadyJoined(chatUser))
            {
                _messageSender.SendDirectMessage(chatUser.DisplayName,
                    "You embarked on this quest ages ago. What have you been doing?!?!");
                return;
            }
            _messageSender.SendMessage(
                $"Well met, {chatUser.DisplayName}! You are embarking on your greatest adventure!");
            _joinedPlayers.Add(chatUser.DisplayName);
        }

        private bool IsAlreadyJoined(ChatUser chatUser)
        {
            return _joinedPlayers.Any(x => x.EqualsIns(chatUser.DisplayName));
        }
    }
}
