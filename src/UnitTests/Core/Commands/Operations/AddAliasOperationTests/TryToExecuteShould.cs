using System;
using DevChatter.Bot.Core.Commands.Operations;
using DevChatter.Bot.Core.Data;
using DevChatter.Bot.Core.Data.Model;
using DevChatter.Bot.Core.Data.Specifications;
using DevChatter.Bot.Core.Events.Args;
using FluentAssertions;
using Moq;
using Xunit;

namespace UnitTests.Core.Commands.Operations.AddAliasOperationTests
{
    public class TryToExecuteShould
    {
        [Fact]
        public void NotSave_GivenMissingAlias()
        {
            var commandReceivedEventArgs = new CommandReceivedEventArgs { Arguments = new[] { "add", "alias" } };
            var mockRepo = new Mock<IRepository>();
            var addAliasOperation = new AddAliasOperation(mockRepo.Object);

            addAliasOperation.TryToExecute(commandReceivedEventArgs);

            mockRepo.Verify(x => x.Create(It.IsAny<CommandWordEntity>()), Times.Never);
        }

        [Fact]
        public void NotSave_GivenMissingExistingWord()
        {
            var commandReceivedEventArgs = new CommandReceivedEventArgs { Arguments = new[] { "add" } };
            var mockRepo = new Mock<IRepository>();
            var addAliasOperation = new AddAliasOperation(mockRepo.Object);

            addAliasOperation.TryToExecute(commandReceivedEventArgs);

            mockRepo.Verify(x => x.Create(It.IsAny<CommandWordEntity>()), Times.Never);
        }

        [Fact]
        public void NotSave_GivenAliasAlreadyUsed()
        {
            var commandReceivedEventArgs = new CommandReceivedEventArgs { Arguments = new[] { "add", "foo", "bar" } };
            var mockRepo = new Mock<IRepository>();
            var addAliasOperation = new AddAliasOperation(mockRepo.Object);
            var existingWord = new CommandWordEntity { CommandWord = Guid.NewGuid().ToString() };
            mockRepo.Setup(x => x.Single(It.IsAny<CommandWordPolicy>())).Returns(existingWord);

            string message = addAliasOperation.TryToExecute(commandReceivedEventArgs);

            mockRepo.Verify(x => x.Create(It.IsAny<CommandWordEntity>()), Times.Never);
            message.Should().Contain(existingWord.CommandWord);
        }

        [Fact]
        public void SaveAlias_GivenValidArguments()
        {
            var newAlias = Guid.NewGuid().ToString();
            var commandReceivedEventArgs = new CommandReceivedEventArgs { Arguments = new[] { "add", "foo", newAlias } };
            var mockRepo = new Mock<IRepository>();
            var addAliasOperation = new AddAliasOperation(mockRepo.Object);
            var existingWord = new CommandWordEntity { CommandWord = Guid.NewGuid().ToString() };
            mockRepo.Setup(x => x.Single(It.IsAny<CommandWordPolicy>()))
                .Returns(existingWord); // call for type to alias
            mockRepo.Setup(x => x.Single(It.IsAny<CommandWordPolicy>()))
                .Returns(null as CommandWordEntity); // check for existing

            string message = addAliasOperation.TryToExecute(commandReceivedEventArgs);

            mockRepo.Verify(x => x.Create(It.IsAny<CommandWordEntity>()), Times.Once);
            message.Should().Contain(newAlias);
        }
    }
}
