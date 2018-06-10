using System;
using System.Collections.Generic;
using System.Text;
using DevChatter.Bot.Core.Data;
using DevChatter.Bot.Core.Data.Model;
using DevChatter.Bot.Core.Events.Args;
using DevChatter.Bot.Core.Systems.Chat;

namespace DevChatter.Bot.Core.Commands
{
    public class WallOfShameCommand : BaseCommand
    {
        public WallOfShameCommand(IRepository repository)
            : base(repository, UserRole.Everyone)
        {
            HelpText = "Helps you understand who are the worst devchatter viewers that havent contributed yet.";
        }
        protected override void HandleCommand(IChatClient chatClient, CommandReceivedEventArgs eventArgs)
        {
            var textToSend = "Least contributions: aceflameseer: 0  BibleThump";
            chatClient.SendMessage(textToSend);
        }
    }
}
