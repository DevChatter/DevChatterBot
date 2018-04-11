using DevChatter.Bot.Core.Data.Model;
using DevChatter.Bot.Core.Events;
using DevChatter.Bot.Core.Events.Args;
using DevChatter.Bot.Core.Messaging.Tokens;
using Xunit;

namespace UnitTests.Core.Messaging.SimpleTokenTests
{
    public class ReplaceCommandValuesShould
    {
        [Fact]
        public void PerformSimpleReplacement()
        {
            const string responseTemplate = "[UserDisplayName] is now lurking!";
            var eventArgs = new CommandReceivedEventArgs {ChatUser = new ChatUser {DisplayName = "Lyon"}};

            string resultingMessage = SimpleToken.UserDisplayName.ReplaceCommandValues(responseTemplate, eventArgs);

            Assert.Equal("Lyon is now lurking!", resultingMessage);
        }

        [Fact]
        public void PerformSimpleReplacement_GivenUnrelatedTokens()
        {
            const string responseTemplate = "[UserDisplayName] is now [CommandWord]ing!";
            var eventArgs = new CommandReceivedEventArgs {ChatUser = new ChatUser {DisplayName = "Lyon"}};

            string resultingMessage = SimpleToken.UserDisplayName.ReplaceCommandValues(responseTemplate, eventArgs);

            Assert.Equal("Lyon is now [CommandWord]ing!", resultingMessage);
        }

        [Fact]
        public void PerformMultipleReplacements_GivenMultipleTokens()
        {
            const string responseTemplate = "[UserDisplayName] is now [CommandWord]ing!";
            var eventArgs =
                new CommandReceivedEventArgs {CommandWord = "lurk", ChatUser = new ChatUser {DisplayName = "Lyon"}};

            string afterTokenReplacement1 =
                SimpleToken.UserDisplayName.ReplaceCommandValues(responseTemplate, eventArgs);
            string afterTokenReplacement2 =
                SimpleToken.CommandWord.ReplaceCommandValues(afterTokenReplacement1, eventArgs);

            Assert.Equal("Lyon is now lurking!", afterTokenReplacement2);
        }
    }
}
