using System;
using System.Collections.Generic;
using DevChatter.Bot.Core.Automation;
using DevChatter.Bot.Core.Games.Heist;
using DevChatter.Bot.Core.Util;
using FluentAssertions;
using Xunit;

namespace UnitTests.Core.Games.Heist.HeistMissionTests
{
    public class AttemptHeistShould
    {
        [Fact]
        public void ReturnNoSurvivors_GivenNoMembers()
        {
            var heistMission = new HeistMission(0, "Something", new List<HeistRoles>(), 0);

            HeistMissionResult heistMissionResult = heistMission.AttemptHeist(new Dictionary<HeistRoles, string>());

            heistMissionResult.SurvivingMembers.Should().BeEmpty();
        }

        //[Fact]
        //public void ReturnAllSurvivors_GivenEasyWin()
        //{
        //    Shim shim = Shim.Replace(() => MyRandom.RandomNumber(0, 100)).With(delegate (int a, int b) { return 99; });

        //    var heistMission = HeistMission.GetById(1);


        //    PoseContext.Isolate(() =>
        //    {
        //        HeistMissionResult heistMissionResult = heistMission.AttemptHeist(new Dictionary<HeistRoles, string>());
        //        heistMissionResult.SurvivingMembers.Should().BeEmpty();
        //    }, shim);

        //}
    }
}
