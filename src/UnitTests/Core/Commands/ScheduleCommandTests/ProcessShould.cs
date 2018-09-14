using DevChatter.Bot.Core.Commands;
using DevChatter.Bot.Core.Data;
using DevChatter.Bot.Core.Data.Model;
using DevChatter.Bot.Core.Data.Specifications;
using DevChatter.Bot.Core.Events.Args;
using DevChatter.Bot.Core.GoogleApi;
using DevChatter.Bot.Core.Systems.Chat;
using Moq;
using System;
using System.Collections.Generic;
using Xunit;

namespace UnitTests.Core.Commands.ScheduleCommandTests
{
    public class ScheduleCommandShould
    {
        private readonly ScheduleCommand _scheduleCommand;
        private readonly Mock<IChatClient> _chatClientMock = new Mock<IChatClient>();
        private readonly Mock<IRepository> _repositoryMock = new Mock<IRepository>();
        private readonly CommandReceivedEventArgs _commandReceivedEventArgs = new CommandReceivedEventArgs();

        public ScheduleCommandShould()
        {
            var entities = new List<ScheduleEntity>
            {
                new ScheduleEntity { ExampleDateTime = new DateTimeOffset(2018, 6, 19, 18, 0, 0, 0, TimeSpan.Zero)},
                new ScheduleEntity { ExampleDateTime = new DateTimeOffset(2018, 6, 21, 16, 0, 0, 0, TimeSpan.Zero)},
            };
            _repositoryMock.Setup(x => x.List(It.IsAny<DataItemPolicy<ScheduleEntity>>()))
                .Returns(entities);
            _scheduleCommand = new ScheduleCommand(_repositoryMock.Object,
                new Mock<ITimezoneLookup>().Object);
        }

        [Theory]
        [InlineData("-19")]
        [InlineData("+999")]
        [InlineData("19")]
        public void SendErrorMessage_GivenInvalidArguments(string argument)
        {
            List<string> arguments = new List<string>
            {
                argument
            };
            _commandReceivedEventArgs.Arguments = arguments;
            _scheduleCommand.Process(_chatClientMock.Object, _commandReceivedEventArgs);
            _chatClientMock.Verify(x => x.SendMessage(ScheduleCommand.Messages.OUT_OF_RANGE));
        }

        [Theory]
        [InlineData("",   "Our usual schedule (at UTC +0) is: Tuesdays at 18:00, Thursdays at 16:00")]
        [InlineData("-4", "Our usual schedule (at UTC -4) is: Tuesdays at 14:00, Thursdays at 12:00")]
        [InlineData("+8", "Our usual schedule (at UTC +8) is: Wednesdays at 02:00, Fridays at 00:00")]
        public void SendMatchingScheduleMessage_GivenValidArguments(string argument, string expectedMessage)
        {
            List<string> arguments = new List<string>();
            if (!string.IsNullOrWhiteSpace(argument))
            {
                arguments.Add(argument);
            }
            _commandReceivedEventArgs.Arguments = arguments;
            _scheduleCommand.Process(_chatClientMock.Object, _commandReceivedEventArgs);
            _chatClientMock.Verify(x => x.SendMessage(expectedMessage));
        }

        //[Theory]
        //[InlineData("GMT", "Our usual schedule (at UTC +0) is: Tuesdays at 6:00 PM, Thursdays at 4:00 PM")]
        //[InlineData("EDT", "Our usual schedule (at UTC -4) is: Tuesdays at 2:00 PM, Thursdays at 12:00 PM")]
        //[InlineData("CST", "Our usual schedule (at UTC +8) is: Wednesdays at 2:00 AM, Fridays at 12:00 AM")]
        //// Abbreviations taken from https://en.wikipedia.org/wiki/List_of_time_zone_abbreviations
        //public void SendMatchingScheduleMessage_GivenValidTimeZoneAbbreviation(string argument, string expectedMessage)
        //{
        //    List<string> arguments = new List<string>
        //    {
        //        argument
        //    };
        //    _commandReceivedEventArgs.Arguments = arguments;
        //    _scheduleCommand.Process(_chatClientMock.Object, _commandReceivedEventArgs);
        //    _chatClientMock.Verify(x => x.SendMessage(expectedMessage));
        //}

        //// Please beware that cities might adjust for daylight savings time and break the test.
        //[Theory]
        //[InlineData("London", "Our usual schedule (at UTC +0) is: Tuesdays at 6:00 PM, Thursdays at 4:00 PM")]
        //[InlineData("Halifax", "Our usual schedule (at UTC -4) is: Tuesdays at 2:00 PM, Thursdays at 12:00 PM")]
        //[InlineData("Singapore", "Our usual schedule (at UTC +8) is: Wednesdays at 2:00 AM, Fridays at 12:00 AM")]
        //// Cities taken from https://en.wikipedia.org/wiki/List_of_UTC_time_offsets
        //public void SendMatchingScheduleMessage_GivenPrincipalCity(string argument, string expectedMessage)
        //{
        //    List<string> arguments = new List<string>
        //    {
        //        argument
        //    };
        //    _commandReceivedEventArgs.Arguments = arguments;
        //    _scheduleCommand.Process(_chatClientMock.Object, _commandReceivedEventArgs);
        //    _chatClientMock.Verify(x => x.SendMessage(expectedMessage));
        //}

    }
}



