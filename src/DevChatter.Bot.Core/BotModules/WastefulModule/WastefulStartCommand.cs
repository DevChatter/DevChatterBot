using DevChatter.Bot.Core.Commands;
using DevChatter.Bot.Core.Data;
using DevChatter.Bot.Core.Events.Args;
using DevChatter.Bot.Core.Systems.Chat;

namespace DevChatter.Bot.Core.BotModules.WastefulModule
{
    public class WastefulStartCommand : BaseCommand
    {
        private readonly IWastefulDisplayNotification _notification;

        public WastefulStartCommand(IRepository repository, IWastefulDisplayNotification notification) : base(repository)
        {
            _notification = notification;
        }

        protected override void HandleCommand(IChatClient chatClient, CommandReceivedEventArgs eventArgs)
        {
            _notification.StartGame(eventArgs.ChatUser.DisplayName);
        }
    }
}
