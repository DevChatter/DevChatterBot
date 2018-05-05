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
            TopCommandsOperation topCommandsOperation = SetUpTest(new List<CommandUsageEntity>());

            string messageResult = topCommandsOperation.TryToExecute(new CommandReceivedEventArgs());

            messageResult.Should().Be(TopCommandsOperation.NO_DATA_MESSAGE);
        }

        [Fact]
        public void ReturnSingleEntry_GivenSameCommandAnalyticsData()
        {
            var fullTypeName = Guid.NewGuid().ToString();
            var entities = new List<CommandUsageEntity> { new CommandUsageEntity { FullTypeName = fullTypeName }, new CommandUsageEntity { FullTypeName = fullTypeName }, new CommandUsageEntity { FullTypeName = fullTypeName } };
            TopCommandsOperation topCommandsOperation = SetUpTest(entities);

            string messageResult = topCommandsOperation.TryToExecute(new CommandReceivedEventArgs());

            messageResult.Should().Contain(fullTypeName);
        }

        [Fact]
        public void DisplayOnlyNameOfType()
        {
            string name = "Foo";
            string notIncluded = "Buzz";
            var entities = new List<CommandUsageEntity> { new CommandUsageEntity { FullTypeName = $"Bar.Fizz.{notIncluded}.{name}" } };
            TopCommandsOperation topCommandsOperation = SetUpTest(entities);

            string messageResult = topCommandsOperation.TryToExecute(new CommandReceivedEventArgs());

            messageResult.Should().Contain(name);
            messageResult.Should().NotContain(notIncluded);
        }

        [Fact]
        public void ReturnMultipleEntries_GivenMultipleCommandsData()
        {
            var fullTypeName1 = Guid.NewGuid().ToString();
            var fullTypeName2 = Guid.NewGuid().ToString();
            var entities = new List<CommandUsageEntity>
            {
                new CommandUsageEntity {FullTypeName = fullTypeName1},
                new CommandUsageEntity {FullTypeName = fullTypeName2}
            };
            TopCommandsOperation topCommandsOperation = SetUpTest(entities);

            string messageResult = topCommandsOperation.TryToExecute(new CommandReceivedEventArgs());

            messageResult.Should().Contain(fullTypeName1);
            messageResult.Should().Contain(fullTypeName2);
        }

        [Fact]
        public void ReturnOnlyTopFiveCommands_GivenDataForMoreThanFiveCommands()
        {
            var fullTypeName = Guid.NewGuid().ToString();
            List<CommandUsageEntity> entities = GetTestEntities(fullTypeName, "1", "2", "3", "4", "5");
            TopCommandsOperation topCommandsOperation = SetUpTest(entities);

            string messageResult = topCommandsOperation.TryToExecute(new CommandReceivedEventArgs());

            messageResult.Should().NotContain(fullTypeName);
        }

        private static TopCommandsOperation SetUpTest(List<CommandUsageEntity> commandUsageEntities)
        {
            var mockRepo = new Mock<IRepository>();
            mockRepo.Setup(x => x.List<CommandUsageEntity>(null)).Returns(commandUsageEntities);
            var topCommandsOperation = new TopCommandsOperation(mockRepo.Object);
            return topCommandsOperation;
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
