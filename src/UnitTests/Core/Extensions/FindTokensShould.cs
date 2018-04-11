using System.Collections.Generic;
using System.Linq;
using DevChatter.Bot.Core.Extensions;
using Xunit;

namespace UnitTests.Core.Extensions
{
    public class FindTokensShould
    {
        [Theory]
        [InlineData("")]
        [InlineData("This has no tokens.")]
        public void ReturnEmptyCollection_GivenNoTokens(string given)
        {
            IEnumerable<string> tokens = given.FindTokens();

            Assert.Empty(tokens);
        }

        [Theory]
        [InlineData("[It] can be anywhere.", "[It]")]
        [InlineData("It [can] be anywhere.", "[can]")]
        [InlineData("It can [be] anywhere.", "[be]")]
        [InlineData("It can be [anywhere].", "[anywhere]")]
        public void ReturnOnlyToken_GivenOneToken(string given, string expectedToken)
        {
            var tokens = given.FindTokens().ToList();

            Assert.Single(tokens);
            Assert.Equal(tokens.Single(), expectedToken);
        }

        [Fact]
        public void ReturnMultipleTokens_GivenMultiTokenMessage()
        {
            string given = "This [message] contains [two] tokens.";
            var tokens = given.FindTokens().ToArray();

            Assert.Equal(new[] {"[message]", "[two]"}, tokens);
        }
    }
}
