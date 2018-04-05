using System;
using System.Linq;
using DevChatter.Bot.Core.Data;
using DevChatter.Bot.Core.Data.Model;
using DevChatter.Bot.Core.Events.Args;
using DevChatter.Bot.Core.Systems.Chat;

namespace DevChatter.Bot.Core.Commands
{
    public class ScheduleCommand : BaseCommand
    {
        public string CommandText => "schedule";

        public ScheduleCommand(IRepository repository) : base(repository, UserRole.Mod)
        {
            HelpText = "To see our schedule just type !schedule and to see it in another timezone, pass your UTC offset. Example: !schedule -4";
        }

        public override void Process(IChatClient chatClient, CommandReceivedEventArgs eventArgs)
        {
            if (!int.TryParse(eventArgs?.Arguments?.ElementAtOrDefault(0), out int offset))
            {
                return;
            }

            // Monday at 2 PM
            DateTime monday = GetInstanceOf(DayOfWeek.Monday, 18, 0);
            DateTime tuesday = GetInstanceOf(DayOfWeek.Tuesday, 18, 0);
            DateTime thursday = GetInstanceOf(DayOfWeek.Thursday, 16, 0);
            DateTime saturday = GetInstanceOf(DayOfWeek.Saturday, 17, 0);
            ////DateTime mondayYourTime = TimeZoneInfo.GetSystemTimeZones().Where(x => x.)
            ////    DateTimeOffset dateTimeOffset = new DateTimeOffset(monday,);
            //chatClient.SendMessage($"{mondayYourTime.ToShortTimeString()}");
        }

        private DateTime GetInstanceOf(DayOfWeek dayOfWeek, int hour, int minutes)
        {
            switch (dayOfWeek)
            {
                case DayOfWeek.Monday:
                    return new DateTime(2018, 4, 2, hour, minutes, 0, DateTimeKind.Utc);
                case DayOfWeek.Tuesday:
                    return new DateTime(2018, 4, 3, hour, minutes, 0, DateTimeKind.Utc);
                case DayOfWeek.Wednesday:
                    return new DateTime(2018, 4, 4, hour, minutes, 0, DateTimeKind.Utc);
                case DayOfWeek.Thursday:
                    return new DateTime(2018, 4, 5, hour, minutes, 0, DateTimeKind.Utc);
                case DayOfWeek.Friday:
                    return new DateTime(2018, 4, 6, hour, minutes, 0, DateTimeKind.Utc);
                case DayOfWeek.Saturday:
                    return new DateTime(2018, 4, 7, hour, minutes, 0, DateTimeKind.Utc);
                case DayOfWeek.Sunday:
                    return new DateTime(2018, 4, 8, hour, minutes, 0, DateTimeKind.Utc);
            }

            return default(DateTime);
        }
    }
}
