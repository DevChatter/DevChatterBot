using System;
using System.Collections.Generic;
using DevChatter.Bot.Core.Commands.Operations;
using DevChatter.Bot.Core.Data;
using DevChatter.Bot.Core.Data.Model;
using DevChatter.Bot.Core.Events.Args;
using FluentAssertions;
using Moq;
using Xunit;

namespace UnitTests.Core.Commands.Operations.TopCommandsOperationTests
{
    public class TryToExecuteShould
    {
        [Fact]
        public void ReturnSpecialMessage_GivenNoAnalyticsDataYet()
        {
            var mockRepo = new Mock<IRepository>();
            mockRepo.Setup(x => x.List<CommandUsageEntity>(null)).Returns(new List<CommandUsageEntity>());
            var topCommandsOperation = new TopCommandsOperation(mockRepo.Object);

            string messageResult = topCommandsOperation.TryToExecute(new CommandReceivedEventArgs());

            messageResult.Should().Be(TopCommandsOperation.NO_DATA_MESSAGE);
        }

        [Fact]
        public void ReturnSingleEntry_GivenSameCommandAnalyticsData()
        {
            var mockRepo = new Mock<IRepository>();
            var fullTypeName = Guid.NewGuid().ToString();
            var entities = new List<CommandUsageEntity> { new CommandUsageEntity { FullTypeName = fullTypeName }, new CommandUsageEntity { FullTypeName = fullTypeName }, new CommandUsageEntity { FullTypeName = fullTypeName } };
            mockRepo.Setup(x => x.List<CommandUsageEntity>(null)).Returns(entities);
            var topCommandsOperation = new TopCommandsOperation(mockRepo.Object);

            string messageResult = topCommandsOperation.TryToExecute(new CommandReceivedEventArgs());

            messageResult.Should().Contain(fullTypeName);
        }

        [Fact]
        public void ReturnMultipleEntries_GivenMultipleCommandsData()
        {
            var mockRepo = new Mock<IRepository>();
            var fullTypeName1 = Guid.NewGuid().ToString();
            var fullTypeName2 = Guid.NewGuid().ToString();
            var entities = new List<CommandUsageEntity>
            {
                new CommandUsageEntity {FullTypeName = fullTypeName1},
                new CommandUsageEntity {FullTypeName = fullTypeName2}
            };
            mockRepo.Setup(x => x.List<CommandUsageEntity>(null)).Returns(entities);
            var topCommandsOperation = new TopCommandsOperation(mockRepo.Object);

            string messageResult = topCommandsOperation.TryToExecute(new CommandReceivedEventArgs());

            messageResult.Should().Contain(fullTypeName1);
            messageResult.Should().Contain(fullTypeName2);
        }

        [Fact]
        public void ReturnOnlyTopFiveCommands_GivenDataForMoreThanFiveCommands()
        {
            var mockRepo = new Mock<IRepository>();
            var fullTypeName = Guid.NewGuid().ToString();
            List<CommandUsageEntity> entities = GetTestEntities(fullTypeName, "1", "2", "3", "4", "5");
            mockRepo.Setup(x => x.List<CommandUsageEntity>(null)).Returns(entities);
            var topCommandsOperation = new TopCommandsOperation(mockRepo.Object);

            string messageResult = topCommandsOperation.TryToExecute(new CommandReceivedEventArgs());

            messageResult.Should().NotContain(fullTypeName);
        }

        private static List<CommandUsageEntity> GetTestEntities(params string[] names)
        {
            var entities = new List<CommandUsageEntity>();

            for (int i = 0; i < names.Length; i++)
            {
                for (int j = 0; j < i+1; j++)
                {
                    entities.Add(new CommandUsageEntity { FullTypeName = names[i] });
                }
            }
            return entities;
        }
    }
}
