using DevChatter.Bot.Core.Commands;
using DevChatter.Bot.Core.Data;
using DevChatter.Bot.Core.Events.Args;
using DevChatter.Bot.Core.Systems.Chat;
using DevChatter.Bot.Modules.WastefulGame.Commands.Operations;
using DevChatter.Bot.Modules.WastefulGame.Data;
using DevChatter.Bot.Modules.WastefulGame.Model.Specifications;
using System.Collections.Generic;
using System.Linq;

namespace DevChatter.Bot.Modules.WastefulGame.Commands
{
    public class ShopCommand : BaseCommand
    {
        private readonly IGameRepository _gameRepository;
        private readonly SurvivorRepo _survivorRepo;
        private readonly List<IGameCommandOperation> _operations;

        public ShopCommand(IRepository repository, IGameRepository gameRepository, SurvivorRepo survivorRepo)
            : base(repository)
        {
            _gameRepository = gameRepository;
            _survivorRepo = survivorRepo;
            _operations = new List<IGameCommandOperation>
            {
                new BuyShopItemOperation(_gameRepository)
            };
        }

        protected override void HandleCommand(IChatClient chatClient,
            CommandReceivedEventArgs eventArgs)
        {
            var survivor = _survivorRepo.GetOrCreate(eventArgs.ChatUser);
            string operand = eventArgs.Arguments.FirstOrDefault();

            var operation = _operations.SingleOrDefault(op => op.ShouldExecute(operand));
            if (operation != null)
            {
                string message = operation.TryToExecute(eventArgs, survivor);
                chatClient.SendMessage(message);
            }
            else
            {
                var items = _gameRepository.List(ShopItemPolicy.All());
                string itemDisplay = string.Join(", ", items.Select(x => $"{x.Id}:{x.Name}-{x.Price}"));
                chatClient.SendMessage($"Buy somethin' will ya! For sale: {itemDisplay}");
            }
        }
    }
}
