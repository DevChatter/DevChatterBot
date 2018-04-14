using System.Collections.Generic;
using DevChatter.Bot.Core.Games.Heist;
using DevChatter.Bot.Core.Util;
using FluentAssertions;
using Xunit;

namespace UnitTests.Core.Util.MyRandomTests
{
    public class ChooseRandomItemShould
    {
        [Fact]
        public void ReturnTypeDefault_GivenEmptySet()
        {
            var (_, role) = MyRandom.ChooseRandomItem(new List<HeistRoles>());

            role.Should().Be(default(HeistRoles));
        }

        [Fact]
        public void ReturnOnlyOption_GivenOneItem()
        {
            var (_, role) = MyRandom.ChooseRandomItem(new List<HeistRoles>{ HeistRoles.Hacker });

            role.Should().Be(HeistRoles.Hacker);
        }

        // TODO: Make this test work somehow...
        //[Fact]
        //public void ReturnAnyOption_GivenMultipleItems()
        //{
        //    HeistRoles notRandomChoice1 = HeistRoles.Hacker;
        //    HeistRoles notRandomChoice2 = HeistRoles.Driver;
        //    var choices = new List<HeistRoles>
        //    {
        //        notRandomChoice1,
        //        notRandomChoice2
        //    };

        //    Shim shim1 = Shim.Replace(() => MyRandom.RandomNumber(0, choices.Count)).With((int a, int b) => 0);
        //    Shim shim2 = Shim.Replace(() => MyRandom.RandomNumber(0, choices.Count)).With((int a, int b) => 1);

        //    HeistRoles chosen1 = default(HeistRoles);
        //    HeistRoles chosen2 = default(HeistRoles);

        //    PoseContext.Isolate(() => chosen1 = MyRandom.ChooseRandomItem(choices), shim1);
        //    PoseContext.Isolate(() => chosen2 = MyRandom.ChooseRandomItem(choices), shim2);

        //    Assert.Equal(notRandomChoice1, chosen1);
        //    Assert.Equal(notRandomChoice2, chosen2);
        //}

    }
}
