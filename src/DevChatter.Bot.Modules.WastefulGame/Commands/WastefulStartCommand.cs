using DevChatter.Bot.Core.Commands;
using DevChatter.Bot.Core.Data;
using DevChatter.Bot.Core.Events.Args;
using DevChatter.Bot.Core.Systems.Chat;
using DevChatter.Bot.Modules.WastefulGame.Hubs.Dtos;
using DevChatter.Bot.Modules.WastefulGame.Model;
using System.Linq;

namespace DevChatter.Bot.Modules.WastefulGame.Commands
{
    public class WastefulStartCommand : BaseCommand
    {
        private readonly SurvivorRepo _gameRepository;
        private readonly IWastefulDisplayNotification _notification;

        public WastefulStartCommand(IRepository repository,
            SurvivorRepo gameRepository,
            IWastefulDisplayNotification notification) : base(repository)
        {
            _gameRepository = gameRepository;
            _notification = notification;
        }

        protected override void HandleCommand(IChatClient chatClient, CommandReceivedEventArgs eventArgs)
        {
            Survivor survivor = _gameRepository.GetOrCreate(eventArgs.ChatUser);
            var inventoryItems = survivor.InventoryItems.Select(HeldItemDto.FromInventoryItem);
            _notification.StartGame(survivor.DisplayName, survivor.UserId, inventoryItems);
        }
    }
}
