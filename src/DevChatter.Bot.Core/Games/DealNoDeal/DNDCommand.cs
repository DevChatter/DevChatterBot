using DevChatter.Bot.Core.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DevChatter.Bot.Core.Commands.Operations;
using DevChatter.Bot.Core.Data;
using DevChatter.Bot.Core.Data.Model;
using DevChatter.Bot.Core.Events;
using DevChatter.Bot.Core.Events.Args;
using DevChatter.Bot.Core.Systems.Chat;

namespace DevChatter.Bot.Core.Games.DealNoDeal
{
    public class DNDCommand : BaseCommand, IGameCommand
    {
        private readonly DealNoDealGame _dealNoDealGame;
        public IGame Game => _dealNoDealGame;

        public DNDCommand(IRepository repository,  DealNoDealGame dealNoDealGame, IChatClient chatClient, ICurrencyGenerator currencyGenerator) : base(repository, UserRole.Everyone)
        {
            _dealNoDealGame = dealNoDealGame;
            _chatClient = chatClient;
            _currencyGenerator = currencyGenerator;
            HelpText =
                "Use \"!dnd\" to start a game. Use \"!dnd pick x\" to pick/guess a box. Use \"!dnd accept\" or \"!dnd decline\"  to accept or decline offers .";
        }
        private List<ICommandOperation> _operations;
        private IChatClient _chatClient;
        private ICurrencyGenerator _currencyGenerator;

        public List<ICommandOperation> Operations => _operations ?? (_operations = new List<ICommandOperation>
        {
           new PickABoxOperation(_dealNoDealGame,_chatClient,_currencyGenerator),
           new MakeADealOperation(_dealNoDealGame,_chatClient)
        });

        protected override void HandleCommand(IChatClient chatClient, CommandReceivedEventArgs eventArgs)
        {
            string argumentOne = eventArgs?.Arguments?.FirstOrDefault();
            ChatUser chatUser = eventArgs?.ChatUser;
            //dnd start dnd pick
            var operationToUse = Operations.SingleOrDefault(x => x.ShouldExecute(argumentOne));
            if (operationToUse != null)
            {
                string resultMessage = operationToUse.TryToExecute(eventArgs);  
            }
            else
            {
                if (string.IsNullOrWhiteSpace(argumentOne))
                {
                    // attempting to start a game
                    _dealNoDealGame.AttemptToStartGame(chatUser);
                }
            }
        }

    }
}
