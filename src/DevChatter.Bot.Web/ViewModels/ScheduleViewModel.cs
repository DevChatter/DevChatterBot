using System;
using System.ComponentModel.DataAnnotations;
using DevChatter.Bot.Core.Data.Model;

namespace DevChatter.Bot.Web.ViewModels
{
    public class ScheduleViewModel
    {
        public Guid Id { get; set; }
        [DisplayFormat(DataFormatString = "{0:dddd HH:mm tt}")]
        public DateTimeOffset ExampleDateTime { get; set; }

        public static ScheduleViewModel FromScheduleEntity(ScheduleEntity entity)
        {
            return new ScheduleViewModel
            {
                Id = entity.Id,
                ExampleDateTime = entity.ExampleDateTime,
            };
        }

    }
}
