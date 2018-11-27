using DevChatter.Bot.Core.Commands;
using DevChatter.Bot.Core.Data;
using DevChatter.Bot.Core.Events.Args;
using DevChatter.Bot.Core.Systems.Chat;
using System.Collections.Generic;
using System.Linq;

namespace DevChatter.Bot.Modules.WastefulGame.Commands
{
    public class WastefulMoveCommand : BaseCommand
    {
        private readonly List<string> _validDirections =
            new List<string> { "left", "right", "up", "down", "l", "r", "u", "d" };
        private readonly IWastefulDisplayNotification _notification;

        public WastefulMoveCommand(IRepository repository,
            IWastefulDisplayNotification notification) : base(repository)
        {
            _notification = notification;
        }

        protected override void HandleCommand(IChatClient chatClient, CommandReceivedEventArgs eventArgs)
        {
            string direction = eventArgs.Arguments.FirstOrDefault()?.ToLower();
            string moveNumberText = eventArgs.Arguments.ElementAtOrDefault(1) ?? "1";
            if (!int.TryParse(moveNumberText, out int moveNumber))
            {
                moveNumber = 1;
            }
            if (_validDirections.Contains(direction)
                && moveNumber > 0 && moveNumber < 10)
            {
                // TODO: Turn this back on when we can respond in a room.
                //chatClient.SendMessage($"Moving {direction}...", eventArgs.RoomId);
                _notification.MovePlayer(direction, moveNumber);
            }
            else
            {
                // TODO: Give some kind of feedback about the invalid movement request.
            }
        }
    }
}
