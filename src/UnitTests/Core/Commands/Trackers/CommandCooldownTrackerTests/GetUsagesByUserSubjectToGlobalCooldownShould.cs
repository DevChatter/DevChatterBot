using DevChatter.Bot.Core.Commands.Trackers;
using DevChatter.Bot.Core.Events;
using FluentAssertions;
using System;
using Xunit;

namespace UnitTests.Core.Commands.Trackers.CommandCooldownTrackerTests
{
    public class GetUsagesByUserSubjectToGlobalCooldownShould
    {
        private string _testUser1 = "brendan";

        [Fact]
        public void ReturnEmptyCollection_GivenNoCommandsUsed()
        {
            var tracker = new CommandCooldownTracker(new CommandHandlerSettings());

            var usages = tracker.GetUsagesByUserSubjectToGlobalCooldown(_testUser1, DateTimeOffset.UtcNow);

            usages.Should().BeEmpty();
        }

        [Fact]
        public void ReturnSingleItem_GivenOneRecentCommandUsage()
        {
            var tracker = new CommandCooldownTracker(new CommandHandlerSettings {GlobalCommandCooldown = 1});
            DateTimeOffset currentTime = DateTimeOffset.UtcNow;
            tracker.RecordUsage(new CommandUsage(_testUser1, currentTime, null));

            var usages = tracker.GetUsagesByUserSubjectToGlobalCooldown(_testUser1, currentTime);

            usages.Should().HaveCount(1);
        }

        [Fact]
        public void ReturnEmptyCollection_GivenOneOldCommandUsage()
        {
            var tracker = new CommandCooldownTracker(new CommandHandlerSettings {GlobalCommandCooldown = 1});
            DateTimeOffset currentTime = DateTimeOffset.UtcNow;
            DateTimeOffset timeUsageWasInvoked = currentTime.AddSeconds(-2);
            tracker.RecordUsage(new CommandUsage(_testUser1, timeUsageWasInvoked, null));

            var usages = tracker.GetUsagesByUserSubjectToGlobalCooldown(_testUser1, currentTime);

            usages.Should().BeEmpty();
        }
    }
}
