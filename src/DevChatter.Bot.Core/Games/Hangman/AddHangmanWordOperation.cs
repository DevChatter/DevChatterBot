using System.Collections.Generic;
using System.Linq;
using DevChatter.Bot.Core.Data;
using DevChatter.Bot.Core.Data.Model;
using DevChatter.Bot.Core.Data.Specifications;
using DevChatter.Bot.Core.Events.Args;

namespace DevChatter.Bot.Core.Commands.Operations
{
    public class AddHangmanWordOperation : BaseCommandOperation
    {
        private readonly IRepository _repository;

        public AddHangmanWordOperation(IRepository repository)
        {
            _repository = repository;
        }

        public override List<string> OperandWords { get; } = new List<string> { "add" };
        public override string HelpText { get; } = "Use \"!hangman add word\" to add an \"word\" as a new word.";

        public override string TryToExecute(CommandReceivedEventArgs eventArgs)
        {
            if (!eventArgs.ChatUser.IsInThisRoleOrHigher(UserRole.Mod))
            {
                return $"Sorry, {eventArgs.ChatUser.DisplayName}, only mods can add new words.";
            }
            var word = eventArgs?.Arguments?.ElementAtOrDefault(1)?.ToLowerInvariant();

            if (string.IsNullOrEmpty(word))
            {
                return HelpText;
            }

            HangmanWord existingWord = _repository.Single(HangmanWordPolicy.ByWord(word));
            if (existingWord != null)
            {
                return $"The word \"{existingWord.Word}\" already exists in the hangman database.";
            }

            _repository.Create(new HangmanWord(word));

            return $"Added \"{word}\" to the hangman dictionary.";
        }
    }
}
