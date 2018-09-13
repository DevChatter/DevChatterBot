using System;
using System.Collections.Generic;
using System.Linq;
using DevChatter.Bot.Core.Automation;
using DevChatter.Bot.Core.BotModules.DuelingModule.Model;
using DevChatter.Bot.Core.Data.Model;
using DevChatter.Bot.Core.Events.Args;
using DevChatter.Bot.Core.Extensions;
using DevChatter.Bot.Core.Systems.Chat;

namespace DevChatter.Bot.Core.BotModules.DuelingModule
{
    public class DuelingSystem
    {
        private readonly IChatClient _chatClient;

        public DuelingSystem(IChatClient chatClient, IAutomatedActionSystem automatedActionSystem)
        {
            _chatClient = chatClient;
            _chatClient.OnWhisperReceived += ChatClientOnOnWhisperReceived;
            var repeatingCallbackAction = new RepeatingCallbackAction(CheckForExpiredDuels);
            automatedActionSystem.AddAction(repeatingCallbackAction);
        }

        private void CheckForExpiredDuels()
        {
            List<Duel> expiredDuels = _ongoingDuels.Where(duel => duel.IsExpired()).ToList();
            foreach (Duel expiredDuel in expiredDuels)
            {
                _ongoingDuels.Remove(expiredDuel);
                _chatClient.SendMessage(expiredDuel.GetExpirationMessage());
            }
        }

        private void ChatClientOnOnWhisperReceived(object sender, WhisperReceivedEventArgs e)
        {
            if (!_ongoingDuels.Any()) { return; }
            Duel existingDuel = _ongoingDuels
                .SingleOrDefault(duel => duel.IsExpectingInputFrom(e.FromDisplayName));
            DuelResult duelResult = existingDuel?.ApplySelection(e.FromDisplayName, e.Message);

            if (duelResult.DuelIsOver)
            {
                _ongoingDuels.Remove(existingDuel);
                RecordDuelRecord(existingDuel, duelResult);
            }

            if (!string.IsNullOrWhiteSpace(duelResult.MessageForUser))
            {
                _chatClient.SendDirectMessage(e.FromDisplayName, duelResult.MessageForUser);
            }

            if (!string.IsNullOrWhiteSpace(duelResult.MessageForChat))
            {
                _chatClient.SendMessage(duelResult.MessageForChat);
            }
        }

        private void RecordDuelRecord(Duel existingDuel, DuelResult duelResult)
        {
            // TODO: Get a Repo and store this stuff...


            var duelPlayed = new DuelPlayed
            {
                DateDueled = DateTime.UtcNow,
                DuelType = "RPS",
                PlayerRecords = new List<DuelPlayerRecord>
                {
                    new DuelPlayerRecord
                    {
                        UserDisplayName = existingDuel.Challenger.DisplayName,
                        UserId = existingDuel.Challenger.UserId,
                        WinLossTie = GetWinLossTie(existingDuel.Challenger)
                    },
                    new DuelPlayerRecord
                    {
                        UserDisplayName = existingDuel.Opponent.DisplayName,
                        UserId = existingDuel.Opponent.UserId,
                        WinLossTie = GetWinLossTie(existingDuel.Opponent)
                    },
                }
            };

            string GetWinLossTie(ChatUser user)
            {
                throw new NotImplementedException();
            }
        }

        private readonly List<Duel> _ongoingDuels = new List<Duel>();

        public Duel GetChallenges(ChatUser challenger, ChatUser opponent)
        {
            return _ongoingDuels
                .SingleOrDefault(x => x.Opponent == challenger
                                      && x.Challenger == opponent);
        }

        public bool RequestDuel(ChatUser challenger, ChatUser opponent)
        {
            if (IsUserInAnotherDuel(challenger))
            {
                _chatClient.SendMessage($"Sorry, {challenger}, you are already in another duel.");
                return false;
            }

            if (IsUserInAnotherDuel(opponent))
            {
                _chatClient.SendMessage($"Sorry, {opponent} is in another duel already.");
                return false;
            }

            _ongoingDuels.Add(new Duel { Challenger = challenger, Opponent = opponent });
            return true;
        }

        private bool IsUserInAnotherDuel(ChatUser chatUser)
        {
            return _ongoingDuels.Any(x => x.Challenger == chatUser
                                          || x.Opponent == chatUser);
        }

        public void Accept(Duel existingChallenge)
        {
            existingChallenge.Start();
            var startMessage = "Choose your weapon by replying to this with: Rock, Paper, Or Scissors.";
            _chatClient.SendDirectMessage(existingChallenge.Challenger.DisplayName, startMessage);
            _chatClient.SendDirectMessage(existingChallenge.Opponent.DisplayName, startMessage);
        }
    }
}
