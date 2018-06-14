using System;
using System.Collections.Generic;
using System.Text;
using DevChatter.Bot.Core.Commands.Operations;
using DevChatter.Bot.Core.Events;
using DevChatter.Bot.Core.Events.Args;
using DevChatter.Bot.Core.Systems.Chat;

namespace DevChatter.Bot.Core.Games.DealNoDeal
{
    public class MakeADealOperation : BaseCommandOperation
    {
        private DealNoDealGame _dealNoDealGame;
        private IChatClient _chatClient;

        public MakeADealOperation(DealNoDealGame dealNoDealGame, IChatClient chatClient)
        {
            _chatClient = chatClient;
            _dealNoDealGame = dealNoDealGame;
        }

        public override List<string> OperandWords { get; } = new List<string>() {"accept", "decline"};
        public override string HelpText { get; } = "Use \"!dnd accept\" or \"!dnd decline\" to accept or decline the offer.";

        public override string TryToExecute(CommandReceivedEventArgs eventArgs)
        {
            if (!_dealNoDealGame.IsRunning)
            {
                return null;
            }

            bool isMainPlayer = _dealNoDealGame._MainPlayer != eventArgs.ChatUser;
            string pick = eventArgs.Arguments[0];
            if (string.IsNullOrWhiteSpace(pick))
            {
                _chatClient.SendMessage(HelpText);
                return null;
            }
            if (_dealNoDealGame.GameState == DealNoDealGameState.MAKING_A_DEAL)
            {
                //Note: Only other players can choose a starting box
                if (isMainPlayer)
                {
 
                    if (_dealNoDealGame.DealOffer == 0)
                    {
                        _chatClient.SendMessage("Something went wrong, the deal offer is 0");
                        return "";
                    }

                    if (pick == "accept")
                    {
                       _dealNoDealGame.AcceptDeal(_chatClient);
                        return "Deal Accepted";
                    }
                    else if(pick == "decline")
                    {
                        _dealNoDealGame.DeclineDeal(_chatClient);
                        return "Deal Declined";
                    }
                }
            }
            return "";
        }
    }
}
