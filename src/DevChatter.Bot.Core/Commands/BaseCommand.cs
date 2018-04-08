using System.Collections.Generic;
using System.Linq;
using DevChatter.Bot.Core.Data;
using DevChatter.Bot.Core.Data.Model;
using DevChatter.Bot.Core.Data.Specifications;
using DevChatter.Bot.Core.Events;
using DevChatter.Bot.Core.Extensions;
using DevChatter.Bot.Core.Systems.Chat;

namespace DevChatter.Bot.Core.Commands
{
    public abstract class BaseCommand : IBotCommand
    {
	    private readonly IRepository _repository;
	    private readonly bool _isEnabled;
        public UserRole RoleRequired { get; }
        public string PrimaryCommandText => CommandWords.First();
        public IList<string> CommandWords { get; private set; }
        public string HelpText { get; protected set; }

        protected BaseCommand(IRepository repository, UserRole roleRequired)
            : this(repository, roleRequired, true)
        {
        }

	    protected BaseCommand(IRepository repository, UserRole roleRequired, bool isEnabled)
        {
	        _repository = repository;
			RoleRequired = roleRequired;
            _isEnabled = isEnabled;
	        CommandWords = RefreshCommandWords();
		}

	    private string[] RefreshCommandWords()
	    {
		    return _repository
			    .List(CommandWordPolicy.ByType(GetType()))
			    .OrderByDescending(x => x.IsPrimary)
			    .Select(word => word.CommandWord)
			    .ToArray();
	    }

	    public void NotifyWordsModified() => CommandWords = RefreshCommandWords(); 

		public bool ShouldExecute(string commandText) => _isEnabled && CommandWords.Any(x => x.EqualsIns(commandText));

        public abstract void Process(IChatClient chatClient, CommandReceivedEventArgs eventArgs);
    }
}