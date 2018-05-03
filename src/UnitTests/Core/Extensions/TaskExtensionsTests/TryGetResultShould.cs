using DevChatter.Bot.Core.Exceptions;
using DevChatter.Bot.Core.Extensions;
using FluentAssertions;
using System;
using System.Threading.Tasks;
using Xunit;

namespace UnitTests.Core.Extensions.TaskExtensionsTests
{
    public class TryGetResultShould
    {
        [Fact]
        public void DoSomething()
        {
            bool result = TestTask().TryGetResult().Result;

            result.Should().BeTrue();
        }

        [Fact]
        public void DoSomethingElse()
        {
            Action action = () =>
            {
                bool result = TestThrowingTask().TryGetResult().Result;
            };
            action.Should().Throw<RetryException>();
        }

        private async Task<bool> TestTask()
        {
            return await Task.Run(() => true);
        }

        private async Task<bool> TestThrowingTask()
        {
            Func<bool> func = () => throw new Exception("Hi everybody!");
            return await Task.Run(func);
        }
    }
}
