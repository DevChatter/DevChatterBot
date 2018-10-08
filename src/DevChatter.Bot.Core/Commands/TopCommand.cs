using DevChatter.Bot.Core.Data;
using DevChatter.Bot.Core.Data.Model;
using DevChatter.Bot.Core.Events.Args;
using DevChatter.Bot.Core.Systems.Chat;
using System.Collections.Generic;
using System.Linq;

namespace DevChatter.Bot.Core.Commands
{
    public class TopCommand : BaseCommand
    {
        public TopCommand(IRepository repository) : base(repository)
        {
        }

        public void DisplayTopUsers(IChatClient triggeringClient)
        {
            List<ChatUser> topUsers = GetTopUsers();

            string message = GenerateMessage(topUsers);

            triggeringClient.SendMessage(message);
        }

        public static string GenerateMessage(List<ChatUser> topUsers)
        {
            var topUserStrings = topUsers.Select((x, i) => $" devchaHype {i + 1}. {x.DisplayName}:{x.Tokens} ");
            string topUserMessage = string.Join("", topUserStrings);

            return $"This channel's Top Ballers are: {topUserMessage}";
        }

        public List<ChatUser> GetTopUsers()
        {
            return new List<ChatUser>(Repository.List<ChatUser>().Where(u => u.Role != UserRole.Streamer)
                .OrderByDescending(b => b.Tokens).Take(5));
        }

        protected override bool HandleCommand(IChatClient chatClient, CommandReceivedEventArgs eventArgs)
        {
            DisplayTopUsers(chatClient);
        }
    }
}
