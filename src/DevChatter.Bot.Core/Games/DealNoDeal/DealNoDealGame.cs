using DevChatter.Bot.Core.Automation;
using DevChatter.Bot.Core.Data.Model;
using DevChatter.Bot.Core.Events;
using DevChatter.Bot.Core.Systems.Chat;
using System;
using System.Collections.Generic;
using System.Linq;
using DevChatter.Bot.Core.Extensions;

namespace DevChatter.Bot.Core.Games.DealNoDeal
{
    public class DealNoDealGame : IGame
    {
        public bool IsRunning { get; private set; }

        //Configuration
        public const UserRole ROLE_REQUIRED_TO_START = UserRole.Everyone;
        public const int SECONDS_TO_CHOSE_BOXES = 30;
        public const int TOKENS_TO_PLAY = 25;
        public const int MIN_PLAYABLE_BOXES = 5;
        //Change these if you don't like the rewards
        private static readonly List<int> InitialPrices = new List<int>()
        {
            //15 at the moment
            0,1,2,4,6,8,10,15,18,20,25,32,50,64,100
        };

        public List<Box> StartingBoxes;
        public List<Box> BoxesWithOwners;

        public DealNoDealGameState GameState = DealNoDealGameState.GAME_NOT_RUNNING;
        private IIntervalAction _actionBeingPerformed;

        public ChatUser MainPlayer;
        public int DealOffer = 0;

        public DealNoDealGame(IChatClient chatClient, ICurrencyGenerator currencyGenerator, IAutomatedActionSystem automatedActionSystem)
        {
            _chatClient = chatClient;
            _automatedActionFactory = new AutomatedActionFactory(this, _chatClient);
            _currencyGenerator = currencyGenerator;
            _automatedActionSystem = automatedActionSystem;
        }

        private readonly IChatClient _chatClient;
        private readonly ICurrencyGenerator _currencyGenerator;
        private readonly IAutomatedActionSystem _automatedActionSystem;
        private readonly AutomatedActionFactory _automatedActionFactory;


        public void AttemptToStartGame(ChatUser chatUser)
        {

            if (IsRunning)
            {
                SendGameAlreadyStartedMessage(_chatClient, chatUser);
                return;
            }

            if (!chatUser.IsInThisRoleOrHigher(ROLE_REQUIRED_TO_START))
            {
                _chatClient.SendMessage(
                    $"You must be at least a {ROLE_REQUIRED_TO_START} to start a game, {chatUser.DisplayName}");
                return;
            }
            bool transactionComplete = ChargeTokensForStartingAGame(_chatClient, chatUser);
            if (transactionComplete)
            {
                MainPlayer = chatUser;
                IsRunning = true;
                BoxesWithOwners = new List<Box>();
                StartingBoxes = ShuffleBoxes();
                _chatClient.SendMessage(
                    $"{MainPlayer.DisplayName} started a Deal or No Deal game! Please choose a box everyone by typing \"!dnd pick x\" ... Pick a box between numbers 1 and 15");

                GameState = DealNoDealGameState.CHOSING_STARTING_BOXES;
                SetActionForGameState(DealNoDealGameState.CHOSING_STARTING_BOXES);
            }
            else
            {
                _chatClient.SendMessage($"Not enough tokens to start a game, {chatUser}");
            }

        }

        public void SetActionForGameState(DealNoDealGameState gameState)
        {
            if (gameState == DealNoDealGameState.GAME_NOT_RUNNING)
            {
                _automatedActionSystem.RemoveAction(_actionBeingPerformed);
                return;
            }
            //Remove all Game-actions and start a new one
            _automatedActionSystem.RemoveAction(_actionBeingPerformed);
            _actionBeingPerformed = _automatedActionFactory.GetIntervalAction(gameState);
            _automatedActionSystem.AddAction(_actionBeingPerformed);
        }

        public void QuitGame(IChatClient chatClient)
        {
            IsRunning = false;
            MainPlayer = null;
            DealOffer = 0;
            GameState = DealNoDealGameState.GAME_NOT_RUNNING;
            SetActionForGameState(DealNoDealGameState.GAME_NOT_RUNNING);
        }
        private void SendGameAlreadyStartedMessage(IChatClient chatClient, ChatUser chatUser)
        {
            chatClient.SendMessage(
                $"There is already a {nameof(DealNoDealGame)} running by {MainPlayer.DisplayName}, {chatUser.DisplayName}. Wait for the game to finish!");
        }

        private void SendGameNotStartedMessage(IChatClient chatClient, ChatUser chatUser)
        {
            chatClient.SendMessage($"There's no {nameof(DealNoDealGame)} running, {chatUser.DisplayName}.");
        }
        private bool ChargeTokensForStartingAGame(IChatClient chatClient, ChatUser chatUser)
        {
            chatClient.SendMessage($"{TOKENS_TO_PLAY} tokens are being charged for starting a game.");
            return _currencyGenerator.RemoveCurrencyFrom(chatUser.DisplayName, TOKENS_TO_PLAY);
        }
        public void UpdateGameState()
        {
            if (CheckIfGameIsWon())
            {
                return;
            }

            //Note: if the box count is between 2 and totalcount - 2
            int totalPlayableBoxesCount = InitialPrices.Count - StartingBoxes.Count;
            bool shouldOfferDeal = (BoxesWithOwners.Count < totalPlayableBoxesCount - 2) &&
                                   (BoxesWithOwners.Count != 1) &&
                                   (BoxesWithOwners.Count % 2 == 0);
            if (shouldOfferDeal)
            {
                DealOffer = GetADeal();
                GameState = DealNoDealGameState.MAKING_A_DEAL;
                SetActionForGameState(DealNoDealGameState.MAKING_A_DEAL);
            }
            else
            {
                //Restart timer
                GameState = DealNoDealGameState.PICKING_BOXES;
                SetActionForGameState(DealNoDealGameState.PICKING_BOXES);
            }
        }

        //Note: We can randomize this as we please
        private int GetADeal()
        {
            //average
            int Result = BoxesWithOwners.Sum(b => b.TokenValue) / BoxesWithOwners.Count;

            //randomize offer
            Random r = new Random();
            bool shouldSubtract = r.Next(2) == 0;
            int value = r.Next(0, 5);
            //if the offer is 10. the offer would be subtracted or added either 5 or 15
            if (shouldSubtract)
            {
                Result -= value;
            }
            else
            {
                Result += value;
            }

            //Ensure offer is > 0
            if (Result <= 0)
            {
                Result = 1;
            }
            return Result;
        }
        public void AcceptDeal(IChatClient chatClient)
        {
            _currencyGenerator.AddCurrencyTo(MainPlayer.DisplayName, DealOffer);
            chatClient.SendMessage($" {MainPlayer.DisplayName} ACCEPTED THE OFFER!");
            chatClient.SendMessage($"GAME OVER! {MainPlayer.DisplayName} recieved {DealOffer} tokens!");
            DealOffer = 0;
            QuitGame(chatClient);
        }
        public void DeclineDeal(IChatClient chatClient)
        {
            chatClient.SendMessage($"{MainPlayer.DisplayName} declined the Deal of  {DealOffer} tokens!");
            DealOffer = 0;
            GameState = DealNoDealGameState.PICKING_BOXES;
            SetActionForGameState(DealNoDealGameState.PICKING_BOXES);
        }

        public bool CheckIfGameIsWon()
        {
            if (BoxesWithOwners.Count != 1)
            {
                return false;
            }

            int lastTokens = BoxesWithOwners.Single().TokenValue;
            _chatClient.SendMessage($"GAME FINISHED!");
            _chatClient.SendMessage($"A LAST BOX WITH {lastTokens} TOKENS REMAINS!");
            _chatClient.SendMessage($"{MainPlayer.DisplayName} recieved {lastTokens} tokens! ");
            _currencyGenerator.AddCurrencyTo(MainPlayer.DisplayName, lastTokens);
            QuitGame(_chatClient);
            return true;
        }

        private List<Box> ShuffleBoxes()
        {
            List<Box> shuffledBoxes = new List<Box>();
            List<int> pricesToShfufle = new List<int>(InitialPrices);
            Random r = new Random();
            int totalBoxesCount = InitialPrices.Count;

            for (int i = 0; i < totalBoxesCount; i++)
            {
                //get a new random with a range of left indexes
                int indexToRemoveAt = r.Next(totalBoxesCount - i);
                shuffledBoxes.Add(new Box(i, pricesToShfufle[indexToRemoveAt]));
                pricesToShfufle.RemoveAt(indexToRemoveAt);
            }
            return shuffledBoxes;
        }

        public void PickRandomBox(string displayName)
        {
            Random randy = new Random();
            Box randomBox = BoxesWithOwners[randy.Next(BoxesWithOwners.Count)];
            // TODO: Do something with this variable or don't store it here.
            string boxValue = PickABox(randomBox.Owner, displayName);
        }

        public string PickABox(string namePicked, string displayName)
        {
            Box chosenBox = BoxesWithOwners.Find(b => b.Owner.EqualsIns(namePicked));
            if (chosenBox == null)
            {
                _chatClient.SendMessage("That user does not hold a box, please chose another one");
                return null;
            }

            //OpenBox
            _chatClient.SendMessage($"{displayName}, opened a box with a value of {chosenBox.TokenValue}");
            BoxesWithOwners.Remove(chosenBox);
            UpdateGameState();

            return GetBoxValue(chosenBox.TokenValue);
        }

        public void PrintBoxesRemaining()
        {
            string allPrices = "Prices left: ";

            //Printe all the prices left in order
            foreach (var boxPrice in InitialPrices)
            {
                if (BoxesWithOwners.SingleOrDefault(b => b.TokenValue == boxPrice) != null)
                {
                    allPrices += $"[{boxPrice}]";
                }
            }
            _chatClient.SendMessage(allPrices);

            //print all randomized boxes remaining
            string allBoxes = $"Boxes remaining: {BoxesWithOwners.Count}. choose from the following: ";
            foreach (var box in BoxesWithOwners)
            {
                allBoxes += "[" + box.Owner + "] ";
            }
            allBoxes += $" type \"!dnd pick x\" ";
            _chatClient.SendMessage(allBoxes);
        }
        public void EnsureMinPlayableBoxes()
        {
            if (StartingBoxes.Count <= 0) return;
            if (BoxesWithOwners.Count > MIN_PLAYABLE_BOXES) return;
            Random randy = new Random();
            for (int i = BoxesWithOwners.Count; i < 5; i++)
            {
                int index = randy.Next(StartingBoxes.Count);
                Box box = StartingBoxes[index];
                BoxesWithOwners.Add(new Box(box.Id, box.TokenValue, "RandomChoice" + (i + 1)));
                StartingBoxes.Remove(box);
            }
        }
        public string GetBoxValue(int tokenValue)
        {
            /*
             * 33% chance that you will get lied.
             * The tokenvalue will either be between some values or the exact one
             */
            string result = "";
            Random randy = new Random();
            bool shouldLie = randy.Next(3) == 0;
            if (shouldLie)
            {
                //Lie to the user
                var randomBoxValue = BoxesWithOwners[randy.Next(BoxesWithOwners.Count)].TokenValue;
                result = GetAproximateValue(randomBoxValue);
            }
            else
            {
                //dont lie but tell him aproximately
                result = GetAproximateValue(tokenValue);
            }
            return result;
        }

        public string GetAproximateValue(int value)
        {
            //NOTE: This just spits out a value around -20 value +20   or the value
            string result = "";
            Random randy = new Random();

            int r = randy.Next(10);
            //every 4th value should be concrete
            if (r % 4 == 0)
            {
                //return the exact one
                return value.ToString();
            }

            //return aproximate value
            int lowerValue = value - r - randy.Next(10);
            if (lowerValue < 0)
            {
                lowerValue = 0;
            }

            int higherValue = value + r + randy.Next(10);
            result = "Between " + (lowerValue).ToString() + " - " + (higherValue).ToString();

            return result;

        }
    }
}
