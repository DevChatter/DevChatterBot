using System;
using System.Collections.Generic;
using DevChatter.Bot.Core.ChatSystems;
using DevChatter.Bot.Core.Events;
using DevChatter.Bot.Core.Model;
using DevChatter.Bot.Core.Streaming;

namespace DevChatter.Bot.Core.Commands
{
    public class FollowerCommand : SimpleCommand
    {
        private IFollowerService _followerService;
        protected readonly Func<CommandReceivedEventArgs, string> _selector;

        public FollowerCommand()
        {
        }

        public FollowerCommand(string commandText, string staticResponse,
            UserRole userRole = UserRole.Everyone,
            DataItemStatus dataItemStatus = DataItemStatus.Draft,
            Func<CommandReceivedEventArgs, string> selector = null)
            : base(commandText, staticResponse, userRole, dataItemStatus)
        {
            _selector = selector;
        }

        public void Initialize(IFollowerService followerService)
        {
            _followerService = followerService;
        }


        public override void Process(IChatClient triggeringClient, CommandReceivedEventArgs eventArgs)
        {
            string textToSend = _staticResponse;
            if (_selector != null)
            {
                string selectedValue = _selector(eventArgs);
                if (selectedValue == null)
                {
                    List<string> usersWeFollow = _followerService.GetUsersWeFollow();
                    var random = new Random();
                    int randomIndex = random.Next(usersWeFollow.Count);
                    selectedValue = usersWeFollow[randomIndex];
                }
                textToSend = string.Format(textToSend, selectedValue);
            }
            triggeringClient.SendMessage(textToSend);
        }
    }
}