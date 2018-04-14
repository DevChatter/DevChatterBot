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
    public class TopBallersCommand : BaseCommand
    {
        private readonly IRepository _repository;

        public TopBallersCommand(IRepository repository)
            : base(repository, UserRole.Everyone)
        {
            HelpText = "List the biggest ballers in the chat!";
            _repository = repository;
        }

        public override void Process(IChatClient chatClient, CommandReceivedEventArgs eventArgs)
        {
            DisplayTopBallers(chatClient);
        }

        private void DisplayTopBallers(IChatClient triggeringClient)
        {
            List<ChatUser> ballers = TopFiveBallers();

            string message = GenerateMessage(ballers);

            triggeringClient.SendMessage(message);
        }

        public string GenerateMessage(List<ChatUser> ballers)
        {
            string message = $"This channel's Top Ballers are: ";
            for (int i = 0; i < ballers.Count; i++)
            {
                message += $"-|-   {i+1}. {ballers[i].DisplayName}:{ballers[i].Tokens}   ";
            }

            return message;
        }

        public List<ChatUser> TopFiveBallers()
        {
            return new List<ChatUser>(_repository.List<ChatUser>().OrderByDescending(b => b.Tokens).Take(5));
        }
    }
}
