using DevChatter.Bot.Core.Data;
using DevChatter.Bot.Core.Data.Model;
using DevChatter.Bot.Core.Events.Args;
using DevChatter.Bot.Core.Systems.Chat;
using NodaTime;
using System.Collections.Generic;
using System.Linq;
using DevChatter.Bot.Core.Data.Specifications;

namespace DevChatter.Bot.Core.Commands
{
    public class ScheduleCommand : BaseCommand
    {
        public ScheduleCommand(IRepository repository) : base(repository, UserRole.Everyone)
        {
            HelpText = "To see our schedule just type !schedule and to see it in another timezone, pass your UTC offset. Example: !schedule -4";
        }

        protected override void HandleCommand(IChatClient chatClient, CommandReceivedEventArgs eventArgs)
        {

            int offset;
            if (eventArgs.Arguments.Count == 0)
            {
                offset = 0;
            }
            else
            {
                if (!int.TryParse(eventArgs?.Arguments?.ElementAtOrDefault(0), out int chatUserOffset) || chatUserOffset > 18 || chatUserOffset < -18)
                {
                    chatClient.SendMessage("UTC offset must be a whole number between -18 and +18");
                    return;
                }
                offset = chatUserOffset;
            }

            DateTimeZone timeZone = DateTimeZone.ForOffset(Offset.FromHours(offset));

            List<Instant> streamTimes = Repository.List(DataItemPolicy<ScheduleEntity>.All()).Select(x => x.Instant).ToList();

            string message = $"Our usual schedule (at UTC {offset}) is: " + string.Join(", ", streamTimes.Select(x => GetTimeDisplay(x, timeZone)));

            chatClient.SendMessage(message);
        }

        private static string GetTimeDisplay(Instant instant, DateTimeZone timeZone)
        {
            return $"{instant.InZone(timeZone).DayOfWeek}s at {instant.InZone(timeZone).TimeOfDay:h:mm tt}";
        }
    }
}
