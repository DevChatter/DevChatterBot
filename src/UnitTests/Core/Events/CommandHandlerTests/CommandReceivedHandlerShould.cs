using System.Collections.Generic;
using DevChatter.Bot.Core.Commands;
using DevChatter.Bot.Core.Data;
using DevChatter.Bot.Core.Data.Model;
using DevChatter.Bot.Core.Events;
using DevChatter.Bot.Core.Systems.Chat;
using UnitTests.Fakes;
using Xunit;

namespace UnitTests.Core.Events.CommandHandlerTests
{
    public class CommandReceivedHandlerShould
    {
        [Fact]
        public void CallProcessOnCommandWhenEnabled()
        {
            var fakeCommand = new FakeCommand(GetFakeRepository(), true);
            CommandHandler commandHandler = GetTestCommandHandler(fakeCommand);

            commandHandler.CommandReceivedHandler(new FakeChatClient(), 
                new CommandReceivedEventArgs { CommandWord = fakeCommand.CommandText});

            Assert.True(fakeCommand.ProcessWasCalled);
        }

        [Fact]
        public void NotCallProcessOnCommandWhenDisabled()
        {
            var fakeCommand = new FakeCommand(GetFakeRepository(), false);
            CommandHandler commandHandler = GetTestCommandHandler(fakeCommand);

            commandHandler.CommandReceivedHandler(new FakeChatClient(), 
                new CommandReceivedEventArgs { CommandWord = fakeCommand.CommandText });

            Assert.False(fakeCommand.ProcessWasCalled);
        }

        private static IRepository GetFakeRepository()
        {
            return new FakeRepo
            {
                ListToReturn = new List<object>
                {
                    new CommandWordEntity
                    {
                        CommandWord = "Fake", 
                        FullTypeName = "UnitTests.Fakes.FakeCommand"
                    }
                }
            };
        }

        private static CommandHandler GetTestCommandHandler(FakeCommand fakeCommand)
        {
            var commandUsageTracker = new CommandUsageTracker(new CommandHandlerSettings());
            var chatClients = new List<IChatClient> { new FakeChatClient() };
            var commandMessages = new List<IBotCommand> { fakeCommand };
            var commandHandler = new CommandHandler(commandUsageTracker, chatClients, new CommandList(commandMessages));
            return commandHandler;
        }
    }
}