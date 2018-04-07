using System;
using System.Collections.Generic;
using System.Linq;
using DevChatter.Bot.Core.Automation;
using DevChatter.Bot.Core.Data.Model;
using DevChatter.Bot.Core.Systems.Chat;
using DevChatter.Bot.Core.Util;

namespace DevChatter.Bot.Core.Games.Heist
{
    public class HeistGame
    {
        private readonly AutomatedActionSystem _automatedActionSystem;
        public bool IsGameRunning { get; private set; }
        private const UserRole ROLE_REQUIRED_TO_START = UserRole.Subscriber;
        private const int HEIST_DELAY_IN_SECONDS = 90;

        private readonly Dictionary<HeistRoles, string> _heistMembers = new Dictionary<HeistRoles, string>();
        private DelayedMessageAction _lastCallToJoin;
        private OneTimeCallBackAction _startHeistAction;

        public HeistGame(AutomatedActionSystem automatedActionSystem)
        {
            _automatedActionSystem = automatedActionSystem;
        }

        public void AttemptToCreateGame(IChatClient chatClient, ChatUser chatUser)
        {
            if (IsGameRunning) return;

            // role check here
            if (!chatUser.CanUserRunCommand(ROLE_REQUIRED_TO_START))
            {
                chatClient.SendMessage($"Sorry, {chatUser.DisplayName}, but you're not experience enough (sub only) to organize a heist like this one...");
                return;
            }

            // start the game
            IsGameRunning = true;
            chatClient.SendMessage($"{chatUser.DisplayName} is organizing a heist. Type !heist or !heist [role] to join the team!");

            _lastCallToJoin = new DelayedMessageAction(HEIST_DELAY_IN_SECONDS - 30, "Only 30 seconds left to join the heist! Type !heist to join it!", chatClient);
            _automatedActionSystem.AddAction(_lastCallToJoin);

            _startHeistAction = new OneTimeCallBackAction(HEIST_DELAY_IN_SECONDS, () => StartHeist(chatClient));
            _automatedActionSystem.AddAction(_startHeistAction);
        }

        public void StartHeist(IChatClient chatClient)
        {
            _heistMembers.Clear();
            chatClient.SendMessage("Everyone got arrested, because they talked about the heist publicly on a Twitch chat... And then they waited 2 minutes, giving the cops time to catch them. Fools!");
            IsGameRunning = false;
            _automatedActionSystem.RemoveAction(_lastCallToJoin);
            _automatedActionSystem.RemoveAction(_startHeistAction);
        }

        public JoinGameResult AttemptToJoinHeist(string displayName, HeistRoles role)
        {
            if (_heistMembers.ContainsKey(role))
            {
                return HeistJoinResults.RoleTakenResult(displayName, role);
            }

            if (_heistMembers.ContainsValue(displayName))
            {
                return HeistJoinResults.AlreadyInHeistResult(displayName);
            }

            _heistMembers.Add(role, displayName);

            return HeistJoinResults.SuccessJoinResult(displayName, role);
        }
        public JoinGameResult AttemptToJoinHeist(ChatUser chatUser, out HeistRoles role, string displayName)
        {
            bool success;
            (success, role) = MyRandom.ChooseRandomItem(GetAvailableRoles());
            if (!success)
            {
                return HeistJoinResults.HeistFullResult(displayName);
            }

            return AttemptToJoinHeist(displayName, role);
        }

        private List<HeistRoles> GetAvailableRoles()
        {
            var allHeistRoles = Enum.GetValues(typeof(HeistRoles)).Cast<HeistRoles>();
            var claimedRoles = _heistMembers.Keys;
            return allHeistRoles.Except(claimedRoles).ToList();
        }

        public void AttemptToStartGame(IChatClient chatClient)
        {
            if (!GetAvailableRoles().Any() && IsGameRunning)
            {
                _automatedActionSystem.RemoveAction(_startHeistAction);
                StartHeist(chatClient);
            }
        }
    }

    public static class HeistJoinResults
    {
        public static JoinGameResult SuccessJoinResult(string displayName, HeistRoles role) 
            => new JoinGameResult(true, $"{displayName} joined the heist as the {role}!");
        public static JoinGameResult RoleTakenResult(string displayName, HeistRoles role) 
            => new JoinGameResult(false, $"Sorry, {displayName} we already have a {role}!");
        public static JoinGameResult HeistFullResult(string displayName) 
            => new JoinGameResult(false, $"Sorry, {displayName} this heist is full!");
        public static JoinGameResult UnknownRoleResult(string displayName) 
            => new JoinGameResult(false, $"I don't know what role you wanted to be, {displayName}. Try again?");
        public static JoinGameResult AlreadyInHeistResult(string displayName) 
            => new JoinGameResult(false, $"You're already in this heist, {displayName} and you aren't a multi-tasker.");

    }
}