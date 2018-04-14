using System;
using System.Linq;
using DevChatter.Bot.Core.Data;
using DevChatter.Bot.Core.Data.Model;
using DevChatter.Bot.Core.Events.Args;
using DevChatter.Bot.Core.Systems.Chat;
using NodaTime;

namespace DevChatter.Bot.Core.Commands
{
    public class ScheduleCommand : BaseCommand
    {
        public string CommandText => "schedule";

        public ScheduleCommand(IRepository repository) : base(repository, UserRole.Everyone)
        {
            HelpText = "To see our schedule just type !schedule and to see it in another timezone, pass your UTC offset. Example: !schedule -4";
        }

        public override void Process(IChatClient chatClient, CommandReceivedEventArgs eventArgs)
        {
            if (!int.TryParse(eventArgs?.Arguments?.ElementAtOrDefault(0), out int offset) || offset > 18 || offset < -18)
            {
                return;
            }

            DateTimeZone timeZone = DateTimeZone.ForOffset(Offset.FromHours(offset));

            Instant monday = GetInstanceOf(DayOfWeek.Monday, 18, 0);
            Instant tuesday = GetInstanceOf(DayOfWeek.Tuesday, 18, 0);
            Instant thursday = GetInstanceOf(DayOfWeek.Thursday, 16, 0);
            Instant saturday = GetInstanceOf(DayOfWeek.Saturday, 17, 0);

            string message = "Our usual schedule is: "
                        +  " Mondays at " + monday.InZone(timeZone).TimeOfDay
                        + ", Tuesdays at " + tuesday.InZone(timeZone).TimeOfDay
                        + ", Thursdays at " + thursday.InZone(timeZone).TimeOfDay
                        + ", Saturdays at " + saturday.InZone(timeZone).TimeOfDay;

            chatClient.SendMessage(message);
        }

        private Instant GetInstanceOf(DayOfWeek dayOfWeek, int hour, int minutes)
        {
            switch (dayOfWeek)
            {
                case DayOfWeek.Monday:
                    return Instant.FromUtc(2018, 4, 2, hour, minutes);
                case DayOfWeek.Tuesday:
                    return Instant.FromUtc(2018, 4, 3, hour, minutes);
                case DayOfWeek.Wednesday:
                    return Instant.FromUtc(2018, 4, 4, hour, minutes);
                case DayOfWeek.Thursday:
                    return Instant.FromUtc(2018, 4, 5, hour, minutes);
                case DayOfWeek.Friday:
                    return Instant.FromUtc(2018, 4, 6, hour, minutes);
                case DayOfWeek.Saturday:
                    return Instant.FromUtc(2018, 4, 7, hour, minutes);
                case DayOfWeek.Sunday:
                    return Instant.FromUtc(2018, 4, 8, hour, minutes);
            }

            return default(Instant);
        }
    }
}
