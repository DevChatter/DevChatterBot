using System.Collections.Generic;
using System.Linq;
using DevChatter.Bot.Core.Commands;
using DevChatter.Bot.Core.Data;
using DevChatter.Bot.Core.Events.Args;
using DevChatter.Bot.Core.Systems.Chat;

namespace DevChatter.Bot.Core.BotModules.WastefulModule
{ 
    public class WastefulMoveCommand : BaseCommand
    {
        private readonly List<string> _validDirections =
            new List<string> { "left", "right", "up", "down" };
        private readonly IWastefulDisplayNotification _notification;

        public WastefulMoveCommand(IRepository repository,
            IWastefulDisplayNotification notification) : base(repository)
        {
            _notification = notification;
        }

        protected override void HandleCommand(IChatClient chatClient, CommandReceivedEventArgs eventArgs)
        {
            string direction = eventArgs.Arguments.FirstOrDefault()?.ToLower();
            if (_validDirections.Contains(direction))
            {
                // TODO: Turn this back on when we can respond in a room.
                //chatClient.SendMessage($"Moving {direction}...", eventArgs.RoomId);
                _notification.MovePlayer(direction);
            }
        }
    }
}
