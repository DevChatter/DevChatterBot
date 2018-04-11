using DevChatter.Bot.Core.Extensions;
using Xunit;

namespace UnitTests.Core.Extensions
{
    public class NoAtShould
    {
        [Fact]
        public void DoNothing_GivenAtlessString()
        {
            string result = "DevChatter".NoAt();

            Assert.Equal("DevChatter", result);
        }

        [Fact]
        public void RemoveAt_GivenAtPrefixedString()
        {
            string result = "@DevChatter".NoAt();

            Assert.Equal("DevChatter", result);
        }

        [Fact]
        public void Dothing_GivenAtElsewhereInString()
        {
            string result = "Dev@Chatter".NoAt();

            Assert.Equal("Dev@Chatter", result);
        }
    }
}
