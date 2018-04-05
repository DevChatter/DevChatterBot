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

        private Dictionary<HeistRoles, string> _heistMembers = new Dictionary<HeistRoles, string>();

        public HeistGame(AutomatedActionSystem automatedActionSystem)
        {
            _automatedActionSystem = automatedActionSystem;
        }

        public void AttemptToStartGame(IChatClient chatClient, ChatUser chatUser)
        {
            if (!IsGameRunning)
            {
                // role check here
                if (!chatUser.CanUserRunCommand(ROLE_REQUIRED_TO_START))
                {
                    chatClient.SendMessage($"Sorry, {chatUser.DisplayName}, but you're not experience enough (sub only) to organize a heist like this one...");
                    return;
                }

                // start the game
                IsGameRunning = true;
                chatClient.SendMessage($"{chatUser.DisplayName} is organizing a heist. Type !heist or !heist [role] to join the team!");

                var lastCallToJoin = new DelayedMessageAction(HEIST_DELAY_IN_SECONDS - 30, "Only 30 seconds left to join the heist! Type !heist to join it!", chatClient);
                _automatedActionSystem.AddAction(lastCallToJoin);

                var startHeistAction = new OneTimeCallBackAction(HEIST_DELAY_IN_SECONDS, () => StartHeist(chatClient));
                _automatedActionSystem.AddAction(startHeistAction);
            }
        }

        public void StartHeist(IChatClient chatClient)
        {
            chatClient.SendMessage("Everyone got arrested, because they talked about the heist publicly on a Twitch chat... And then they waited 2 minutes, giving the cops time to catch them. Fools!");
            IsGameRunning = false;
        }

        public bool AttemptToJoinHeist(ChatUser chatUser, HeistRoles role)
        {
            if (_heistMembers.ContainsKey(role))
            {
                return false;
            }

            _heistMembers.Add(role, chatUser.DisplayName);

            return true;
        }
        public bool AttemptToJoinHeist(ChatUser chatUser, out HeistRoles role)
        {
            bool success;
            (success, role) = MyRandom.ChooseRandomItem(GetAvailableRoles());
            if (!success)
            {
                return false;
            }

            _heistMembers.Add(role, chatUser.DisplayName);

            return true;
        }

        private List<HeistRoles> GetAvailableRoles()
        {
            return Enum.GetValues(typeof(HeistRoles)).Cast<HeistRoles>().Except(_heistMembers.Keys).ToList();
        }
    }
}
