using System;
using System.Collections.Generic;
using DevChatter.Bot.Core.Commands;
using DevChatter.Bot.Core.Commands.Trackers;
using DevChatter.Bot.Core.Events;
using FluentAssertions;
using Xunit;

namespace UnitTests.Core.Commands.Trackers
{
    // todo fix naming and location of class
    public class CommandListFuzzyMatchingTest
    {
        [Fact]
        public void FindsMatch_GivenPrefixedQuery() // "Hell" is prefix of "Hello"
        {
            var command = new SimpleCommand("Hello", "");

            var list = new CommandList(new List<IBotCommand> { command, });

            var match = list.FindCommandByKeyword("Hell", out _);

            match.Should().Be(command);
        }

        [Fact]
        public void FindsMatch_GivenPostfixedQuery() // "ello" is postfix of "Hello"
        {
            var command = new SimpleCommand("Hello", "");

            var list = new CommandList(new List<IBotCommand> { command, });

            var match = list.FindCommandByKeyword("ello", out _);

            match.Should().Be(match);
        }

        [Fact]
        public void FindsMatch_GivenTwoMisspellings()
        {
            var command = new SimpleCommand("Hello", "");

            var list = new CommandList(
                new List<IBotCommand> { command, },
                2);

            var match = list.FindCommandByKeyword("Halko", out _);

            match.Should().Be(match);
        }

        [Fact]
        public void FindsNull_GivenThreeMisspellings()
        {
            var command = new SimpleCommand("Hello", "");

            var list = new CommandList(
                new List<IBotCommand> { command, },
                2);

            var match = list.FindCommandByKeyword("Halku", out _);

            match.Should().Be(null);
        }

        [Fact]
        public void FindsMatch_GivenThreeMisspellings_AndMaxDistanceSetToThree()
        {
            var command = new SimpleCommand("Hello", "");

            var list = new CommandList(
                new List<IBotCommand> { command, },
                3);

            var match = list.FindCommandByKeyword("Halku", out _);

            match.Should().Be(match);
        }

        [Fact]
        public void FindsNull_GivenUnrelatableQuery()
        {
            var command = new SimpleCommand("Hello", "");

            var list = new CommandList(new List<IBotCommand> { command, });

            var match = list.FindCommandByKeyword("Foo", out _);

            match.Should().Be(null);
        }
    }
}
