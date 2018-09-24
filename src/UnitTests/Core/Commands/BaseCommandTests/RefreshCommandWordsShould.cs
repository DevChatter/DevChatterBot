using DevChatter.Bot.Core.Data;
using DevChatter.Bot.Core.Data.Model;
using DevChatter.Bot.Core.Data.Specifications;
using FluentAssertions;
using Moq;
using System.Collections.Generic;
using System.Linq;
using UnitTests.DataBuilders;
using Xunit;

namespace UnitTests.Core.Commands.BaseCommandTests
{
    public class RefreshCommandWordsShould
    {
        private CommandEntityBuilder _commandBuilder = new CommandEntityBuilder();

        [Fact]
        public void IncludeOnlyCommandWord_GivenNoAliases()
        {
            var repo = new Mock<IRepository>();
            CommandEntity commandEntity = _commandBuilder.WithDefaults().ClearAliases().Build();
            repo.Setup(x => x.Single(It.IsAny<CommandPolicy>())).Returns(commandEntity);

            var baseCommand = new TestBaseCommand(repo.Object);

            baseCommand.CommandWords.Select(x => x.Word).Should().Contain(commandEntity.CommandWord);
        }

        [Fact]
        public void IncludeBasicCommandWord_GivenAliases()
        {
            var repo = new Mock<IRepository>();
            CommandEntity commandEntity = _commandBuilder.WithDefaults().Build();
            repo.Setup(x => x.Single(It.IsAny<CommandPolicy>())).Returns(commandEntity);

            var baseCommand = new TestBaseCommand(repo.Object);

            baseCommand.CommandWords.Select(x => x.Word).Should().Contain(commandEntity.CommandWord);
        }

        [Fact]
        public void IncludeAliases_GivenAliases()
        {
            var repo = new Mock<IRepository>();
            CommandEntity commandEntity = _commandBuilder.WithDefaults().Build();
            repo.Setup(x => x.Single(It.IsAny<CommandPolicy>())).Returns(commandEntity);

            var baseCommand = new TestBaseCommand(repo.Object);

            IEnumerable<string> words = baseCommand.CommandWords.Select(x => x.Word);
            words.Should().Contain(commandEntity.Aliases.Select(a => a.Word));
        }
    }
}
