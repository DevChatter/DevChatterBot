using System;
using System.Collections.Generic;
using DevChatter.Bot.Core.Data.Model;
using DevChatter.Bot.Core.Systems.Chat;

namespace DevChatter.Bot.Games.Mud.Actions
{
    public interface IMudAction
    {
        bool CanExecute(string actionText);
        void Process(IMessageSender messageSender, ChatUser chatUser, IList<string> arguments);
    }
}
