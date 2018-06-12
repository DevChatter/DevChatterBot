using DevChatter.Bot.Core.Commands;
using DevChatter.Bot.Core.Data;
using DevChatter.Bot.Core.Events;
using DevChatter.Bot.Core.Events.Args;
using DevChatter.Bot.Core.Systems.Chat;
using Moq;
using System.Collections.Generic;
using UnitTests.Fakes;
using Xunit;

namespace UnitTests.Core.Commands.ScheduleCommandTests
{
    public class ScheduleCommandShould
    {
        private ScheduleCommand _scheduleCommand;
        private readonly Mock<IChatClient> _chatClientMock = new Mock<IChatClient>();
        private readonly Mock<IRepository> _repositoryMock = new Mock<IRepository>();
        private CommandReceivedEventArgs commandReceivedEventArgs = new CommandReceivedEventArgs();

        [Theory]
        [InlineData("-19")]
        [InlineData("+999")]
        [InlineData("stoptryingtobreakstuff")]
        public void SendErrorMessage_GivenInvalidArguments(string argument)
        {
            _scheduleCommand = new ScheduleCommand(_repositoryMock.Object);
            List<string> arguments = new List<string>
            {
                argument
            };
            commandReceivedEventArgs.Arguments = arguments;
            _scheduleCommand.Process(_chatClientMock.Object, commandReceivedEventArgs);
            _chatClientMock.Verify(x => x.SendMessage("UTC offset must be a whole number between -18 and +19"));
        }

        // 
        [Theory]
        [InlineData("", "Our usual schedule (at UTC +0) is: Mondays at 6:00 PM, Tuesdays at 6:00 PM, Thursdays at 4:00 PM, Saturdays at 5:00 PM")]
        [InlineData("-4", "Our usual schedule (at UTC -4) is: Mondays at 2:00 PM, Tuesdays at 2:00 PM, Thursdays at 12:00 PM, Saturdays at 1:00 PM")]
        [InlineData("+8", "Our usual schedule (at UTC +8) is: Tuesdays at 2:00 AM, Wednesdays at 2:00 AM, Fridays at 12:00 AM, Sundays at 1:00 AM")]
        public void SendMatchingScheduleMessage_GivenValidArguments(string argument, string expectedMessage)
        {
            _scheduleCommand = new ScheduleCommand(_repositoryMock.Object);
            List<string> arguments = new List<string>
            {
                argument
            };
            commandReceivedEventArgs.Arguments = arguments;
            _scheduleCommand.Process(_chatClientMock.Object, commandReceivedEventArgs);
            _chatClientMock.Verify(x => x.SendMessage(expectedMessage));
        }

        //[Theory]
        //[InlineData("GMT", "Our usual schedule (at UTC +0) is: Mondays at 6:00 PM, Tuesdays at 6:00 PM, Thursdays at 4:00 PM, Saturdays at 5:00 PM")]
        //[InlineData("EDT", "Our usual schedule (at UTC -4) is: Mondays at 2:00 PM, Tuesdays at 2:00 PM, Thursdays at 12:00 PM, Saturdays at 1:00 PM")]
        //[InlineData("CST", "Our usual schedule (at UTC +8) is: Tuesdays at 2:00 AM, Wednesdays at 2:00 AM, Fridays at 12:00 AM, Sundays at 1:00 AM")]
        //// Abbreviations taken from https://en.wikipedia.org/wiki/List_of_time_zone_abbreviations
        //public void SendMatchingScheduleMessage_GivenValidTimeZoneAbbreviation(string argument, string expectedMessage)
        //{
        //    _scheduleCommand = new ScheduleCommand(_repositoryMock.Object);
        //    List<string> arguments = new List<string>
        //    {
        //        argument
        //    };
        //    commandReceivedEventArgs.Arguments = arguments;
        //    _scheduleCommand.Process(_chatClientMock.Object, commandReceivedEventArgs);
        //    _chatClientMock.Verify(x => x.SendMessage(expectedMessage));
        //}

        //// Please beware that cities might adjust for daylight savings time and break the test.
        //[Theory]
        //[InlineData("London", "Our usual schedule (at UTC +0) is: Mondays at 6:00 PM, Tuesdays at 6:00 PM, Thursdays at 4:00 PM, Saturdays at 5:00 PM")]
        //[InlineData("Halifax", "Our usual schedule (at UTC -4) is: Mondays at 2:00 PM, Tuesdays at 2:00 PM, Thursdays at 12:00 PM, Saturdays at 1:00 PM")]
        //[InlineData("Singapore", "Our usual schedule (at UTC +8) is: Tuesdays at 2:00 AM, Wednesdays at 2:00 AM, Fridays at 12:00 AM, Sundays at 1:00 AM")]
        // Cities taken from https://en.wikipedia.org/wiki/List_of_UTC_time_offsets
        //public void SendMatchingScheduleMessage_GivenPrincipalCity(string argument, string expectedMessage)
        //{
        //    _scheduleCommand = new ScheduleCommand(_repositoryMock.Object);
        //    List<string> arguments = new List<string>
        //    {
        //        argument
        //    };
        //    commandReceivedEventArgs.Arguments = arguments;
        //    _scheduleCommand.Process(_chatClientMock.Object, commandReceivedEventArgs);
        //    _chatClientMock.Verify(x => x.SendMessage(expectedMessage));
        //}

    }
}


