using System;
using System.Collections.Generic;
using DevChatter.Bot.Core.Events;
using DevChatter.Bot.Core.Model;

namespace DevChatter.Bot.Core.Messaging
{
    public class FollowerCommand : SimpleResponseMessage
    {
        private IFollowerService _followerService;

        public FollowerCommand()
        {
        }

        public FollowerCommand(string commandText, string staticResponse,
            UserRole userRole = UserRole.Everyone,
            DataItemStatus dataItemStatus = DataItemStatus.Draft,
            Func<CommandReceivedEventArgs, string> selector = null)
            : base(commandText, staticResponse, userRole, dataItemStatus, selector)
        {
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