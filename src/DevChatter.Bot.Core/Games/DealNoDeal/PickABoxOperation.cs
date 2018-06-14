using System.Collections.Generic;
using System.Linq;
using DevChatter.Bot.Core.Commands.Operations;
using DevChatter.Bot.Core.Events;
using DevChatter.Bot.Core.Events.Args;
using DevChatter.Bot.Core.Systems.Chat;

namespace DevChatter.Bot.Core.Games.DealNoDeal
{
    /*
     * Picking starting boxes and the MainPlayer choosing a box are the same operation,
     * based on CHOSING_STARTING_BOXES or PICKING_BOXES the proper operations are performed
     * the OperandWords are the same !dnd pick x
     */
    public class PickABoxOperation : BaseCommandOperation
    {
        private DealNoDealGame _dealNoDealGame;
        private IChatClient _chatClient;
        private ICurrencyGenerator _currencyGenerator;


        public PickABoxOperation(DealNoDealGame dealNoDealGame, IChatClient chatClient,
            ICurrencyGenerator currencyGenerator)
        {
            _currencyGenerator = currencyGenerator;
            _chatClient = chatClient;
            _dealNoDealGame = dealNoDealGame;
        }

        public override List<string> OperandWords { get;  } = new List<string>(){"pick"};
        public override string HelpText { get;  } =
            "Use \"!dnd pick x\" to pick/guess a box.";

        public override string TryToExecute(CommandReceivedEventArgs eventArgs)
        {
            if (!_dealNoDealGame.IsRunning)
            {
                return "";
            }

            bool isMainPlayer = _dealNoDealGame._MainPlayer != eventArgs.ChatUser;
            string namePicked = eventArgs.Arguments[1];
            if (string.IsNullOrWhiteSpace(namePicked))
            {
                return "";
            }

            if (_dealNoDealGame.GameState == DealNoDealGameState.CHOSING_STARTING_BOXES)
            {
                //Note: Only other players can choose a starting box
                if (!isMainPlayer)
                {
                    bool alreadyPickedABox = _dealNoDealGame.BoxesWithOwners.Exists(b => b.Owner == eventArgs.ChatUser.DisplayName);
                    if (alreadyPickedABox)
                    {
                        _chatClient.SendDirectMessage(eventArgs.ChatUser.DisplayName, "You already picked a box");
                        return "";
                    }
                    
                    Box chosenBox = _dealNoDealGame.StartingBoxes.Find(b => b.Id == int.Parse(namePicked));
                    if (chosenBox == null)
                    {
                        _chatClient.SendDirectMessage(eventArgs.ChatUser.DisplayName,"That box number is not available, please chose a different number");
                        return "";
                    }
                    else
                    {
                        //AddBox
                        _chatClient.SendDirectMessage(eventArgs.ChatUser.DisplayName, $"Your box value is  {_dealNoDealGame.GetBoxValue(chosenBox.TokenValue)}");
                        chosenBox.SetOwner(eventArgs.ChatUser.DisplayName);
                        _dealNoDealGame.BoxesWithOwners.Add(chosenBox);
                        _dealNoDealGame.StartingBoxes.Remove(chosenBox);
                    }
                }
            }
            if (_dealNoDealGame.GameState == DealNoDealGameState.PICKING_BOXES)
            {
                if (isMainPlayer)
                {
                    //Pick a box
                     _dealNoDealGame.PickABox(namePicked, eventArgs.ChatUser.DisplayName);
                }
            }
            return "";
        }
        
    }
}
