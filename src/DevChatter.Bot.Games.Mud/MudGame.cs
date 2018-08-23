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
        private readonly List<Player> _players = new List<Player>();

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

            var player = new Player
            {
                Name = chatUser.DisplayName,
                InRoom = _dungeonEntrance,
            };
            _players.Add(player);

            _messageSender.SendMessage($"{chatUser.DisplayName}: {player.InRoom.Look()}");
        }

        public void Look(ChatUser chatUser)
        {
            Player player = GetByChatUser(chatUser);
            _messageSender.SendMessage($"{chatUser.DisplayName}: {player.InRoom.Look()}");
        }

        private Player GetByChatUser(ChatUser chatUser)
            => _players.SingleOrDefault(x => x.Name.EqualsIns(chatUser.DisplayName));

        private bool IsAlreadyJoined(ChatUser chatUser)
        {
            return _players.Any(x => x.Name.EqualsIns(chatUser.DisplayName));
        }

        public void Move(ChatUser chatUser, IList<string> arguments)
        {
            string name = chatUser.DisplayName;
            Player player = GetByChatUser(chatUser);

            if (arguments.FirstOrDefault().EqualsIns("North")
                && player.InRoom.NorthRoom != null)
            {
                _messageSender.SendMessage($"{name} moves North.");
                player.InRoom = player.InRoom.NorthRoom;
            }
            else if (arguments.FirstOrDefault().EqualsIns("East")
                     && player.InRoom.EastRoom != null)
            {
                _messageSender.SendMessage($"{name} moves East.");
                player.InRoom = player.InRoom.EastRoom;
            }
            else if (arguments.FirstOrDefault().EqualsIns("South")
                     && player.InRoom.SouthRoom != null)
            {
                _messageSender.SendMessage($"{name} moves South.");
                player.InRoom = player.InRoom.SouthRoom;
            }
            else if (arguments.FirstOrDefault().EqualsIns("West")
                     && player.InRoom.WestRoom != null)
            {
                _messageSender.SendMessage($"{name} moves West.");
                player.InRoom = player.InRoom.WestRoom;
            }
            else
            {
                _messageSender.SendMessage($"{name} reads about movement.");
            }
        }
    }
}
