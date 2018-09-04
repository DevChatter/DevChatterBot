using DevChatter.Bot.Core.Automation;
using DevChatter.Bot.Core.Data;
using DevChatter.Bot.Core.Data.Model;
using DevChatter.Bot.Core.Events;
using DevChatter.Bot.Core.Systems.Chat;
using DevChatter.Bot.Core.Systems.Streaming;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DevChatter.Bot.Core
{
    public class BotMain
    {
        private readonly IRepository _repository;
        private readonly ICommandHandler _commandHandler;
        private readonly IList<IStreamingPlatform> _streamingPlatforms;
        private readonly IAutomatedActionSystem _automatedActionSystem;

        public BotMain(IRepository repository,
            IList<IStreamingPlatform> streamingPlatforms,
            IAutomatedActionSystem automatedActionSystem,
            ICommandHandler commandHandler)
        {
            _repository = repository;
            _streamingPlatforms = streamingPlatforms;
            _automatedActionSystem = automatedActionSystem;
            _commandHandler = commandHandler;
        }

        public async Task Run()
        {
            var connectTasks = new List<Task>();
            foreach (IStreamingPlatform streamingPlatform in _streamingPlatforms)
            {
                connectTasks.Add(streamingPlatform.Connect());
            }
            await Task.WhenAll(connectTasks);

            await _automatedActionSystem.Start();
        }

        public async Task Stop()
        {
            var disconnectTasks = new List<Task>();
            foreach (IStreamingPlatform streamingPlatform in _streamingPlatforms)
            {
                disconnectTasks.Add(streamingPlatform.Disconnect());
            }
            await Task.WhenAll(disconnectTasks);
        }
    }
}
