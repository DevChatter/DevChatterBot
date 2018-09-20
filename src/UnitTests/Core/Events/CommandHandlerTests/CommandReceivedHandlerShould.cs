using System;
using System.Collections.Generic;
using Autofac;
using DevChatter.Bot.Core.Commands;
using DevChatter.Bot.Core.Commands.Trackers;
using DevChatter.Bot.Core.Data;
using DevChatter.Bot.Core.Data.Model;
using DevChatter.Bot.Core.Data.Specifications;
using DevChatter.Bot.Core.Events;
using DevChatter.Bot.Core.Events.Args;
using DevChatter.Bot.Core.Systems.Chat;
using DevChatter.Bot.Core.Util;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using UnitTests.Fakes;
using Xunit;

namespace UnitTests.Core.Events.CommandHandlerTests
{
    public class CommandReceivedHandlerShould
    {
        [Fact]
        public void CallProcessOnCommandWhenEnabled()
        {
            var fakeCommand = new FakeCommand(GetTestRepository(), true);
            CommandHandler commandHandler = GetTestCommandHandler(fakeCommand);

            commandHandler.CommandReceivedHandler(new Mock<IChatClient>().Object,
                new CommandReceivedEventArgs {CommandWord = fakeCommand.CommandText});

            Assert.True(fakeCommand.ProcessWasCalled);
        }

        [Fact]
        public void NotCallProcessOnCommandWhenDisabled()
        {
            var fakeCommand = new FakeCommand(GetTestRepository(), false);
            CommandHandler commandHandler = GetTestCommandHandler(fakeCommand);

            commandHandler.CommandReceivedHandler(new Mock<IChatClient>().Object,
                new CommandReceivedEventArgs {CommandWord = fakeCommand.CommandText});

            Assert.False(fakeCommand.ProcessWasCalled);
        }

        private static IRepository GetTestRepository()
        {
            var mockRepo = new Mock<IRepository>();
            var commandWordEntities = new List<CommandEntity>
            {
                new CommandEntity
                {
                    CommandWord = "Fake",
                    FullTypeName = "UnitTests.Fakes.FakeCommand"
                }
            };

            mockRepo.Setup(x => x.List(It.IsAny<CommandWordPolicy>())).Returns(commandWordEntities);

            return mockRepo.Object;
        }

        private static CommandHandler GetTestCommandHandler(FakeCommand fakeCommand)
        {
            var commandUsageTracker = new CommandCooldownTracker(new CommandHandlerSettings(), new LoggerAdapter<CommandCooldownTracker>(new NullLogger<CommandCooldownTracker>()));
            var chatClients = new List<IChatClient> {new Mock<IChatClient>().Object};
            var commandMessages = new List<IBotCommand> {fakeCommand};
            var commandHandler = new CommandHandler(new Mock<IRepository>().Object, commandUsageTracker, chatClients,
                new CommandList(commandMessages, new Mock<IComponentContext>().Object), new LoggerAdapter<CommandHandler>(new NullLogger<CommandHandler>()));
            return commandHandler;
        }
    }
}
