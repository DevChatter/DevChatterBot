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
            IEnumerable<string> topUserStrings = topUsers.Select((x, i) => $" devchaHype {i + 1}. {x.DisplayName}:{x.Tokens} ");
            var topUserMessage = string.Join("", topUserStrings);

            return $"This channel's Top Ballers are: {topUserMessage}";
        }

        public List<ChatUser> GetTopUsers()
        {
            return new List<ChatUser>(Repository.List<ChatUser>().OrderByDescending(b => b.Tokens).Take(5));
        }
    }
}
