using System.Collections.Generic;
using DevChatter.Bot.Core.Commands;
using DevChatter.Bot.Core.Data;
using DevChatter.Bot.Core.Events.Args;
using DevChatter.Bot.Core.Systems.Chat;
using DevChatter.Bot.Modules.WastefulGame.Model;
using System.Linq;

namespace DevChatter.Bot.Modules.WastefulGame.Commands
{
    public class InventoryCommand : BaseCommand
    {
        private readonly SurvivorRepo _survivorRepo;

        public InventoryCommand(IRepository repository, SurvivorRepo survivorRepo)
            : base(repository)
        {
            _survivorRepo = survivorRepo;
        }

        protected override void HandleCommand(IChatClient chatClient, CommandReceivedEventArgs eventArgs)
        {
            Survivor survivor = _survivorRepo.GetOrCreate(eventArgs.ChatUser);
            var items = survivor.InventoryItems;

            string itemText = items.Any()
                ? string.Join(", ", items.Select(item => $"{item.Name}({item.Uses})"))
                : "none";
            string message = $"You have these items: {itemText}.";
            chatClient.SendDirectMessage(eventArgs.ChatUser.DisplayName, message);
        }
    }
}
