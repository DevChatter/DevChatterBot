using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DevChatter.Bot.Core.Data;
using DevChatter.Bot.Core.Data.Model;
using DevChatter.Bot.Core.Events.Args;
using DevChatter.Bot.Core.Systems.Chat;

namespace DevChatter.Bot.Core.Commands
{
    public class TopCommand : BaseCommand
    {
        public TopCommand(IRepository repository) : base(repository, UserRole.Everyone)
        {
            HelpText = "List the biggest ballers in the chat!";
        }

        public override void Process(IChatClient chatClient, CommandReceivedEventArgs eventArgs)
        {
            DisplayTopUsers(chatClient);
        }

        public void DisplayTopUsers(IChatClient triggeringClient)
        {
            List<ChatUser> topUsers = GetTopUsers();

            string message = GenerateMessage(topUsers);

            triggeringClient.SendMessage(message);
        }

        public string GenerateMessage(List<ChatUser> topUsers)
        {
            string message = $"This channel's Top Ballers are: ";
            for (int i = 0; i < topUsers.Count; i++)
            {
                message += $"-|-   {i+1}. {topUsers[i].DisplayName}:{topUsers[i].Tokens}   ";
            }

            return message;
        }

        public List<ChatUser> GetTopUsers()
        {
            return new List<ChatUser>(Repository.List<ChatUser>().OrderByDescending(b => b.Tokens).Take(5));
        }
    }
}
