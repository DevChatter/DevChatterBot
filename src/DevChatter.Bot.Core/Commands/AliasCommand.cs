using System;
using System.Linq;
using DevChatter.Bot.Core.Data;
using DevChatter.Bot.Core.Data.Model;
using DevChatter.Bot.Core.Data.Specifications;
using DevChatter.Bot.Core.Events;
using DevChatter.Bot.Core.Systems.Chat;

namespace DevChatter.Bot.Core.Commands
{
    public class AliasCommand : BaseCommand
    {
        private readonly IRepository _repository;

        public AliasCommand(IRepository repository) : base(repository, UserRole.Mod)
        {
            _repository = repository;
            HelpText = "Use !alias add !<existing> !<new> to add a new command name, or !alias" +
                       " del !<existing> to delete a command name. For example, \"!alias add hangman " +
                       "hm\" creates a new shorthand for Hang Man.";
        }

        public EventHandler<EventArgs> CommandAliasModified;

        public override void Process(IChatClient chatClient, CommandReceivedEventArgs eventArgs)
        {
            var oper = eventArgs?.Arguments?.ElementAtOrDefault(0)?.ToLowerInvariant();
            var word = eventArgs?.Arguments?.ElementAtOrDefault(1)?.ToLowerInvariant();
            var newAlias = eventArgs?.Arguments?.ElementAtOrDefault(2)?.ToLowerInvariant();

            if (string.IsNullOrEmpty(oper) || string.IsNullOrEmpty(word))
            {
                chatClient.SendMessage(HelpText);
                return;
            }

            var typeName = _repository.Single(CommandWordPolicy.ByWord(word))?.FullTypeName;

            if (typeName == null)
            {
                chatClient.SendMessage($"The command '!{word}' doesn't exist.");
                return;
            }

            switch (oper)
            {
                case "add":
                    AddWord(chatClient, word, newAlias, typeName);
                    CommandAliasModified?.Invoke(this, EventArgs.Empty);
                    return;
                case "del":
                    DeleteWord(chatClient, word);
                    CommandAliasModified?.Invoke(this, EventArgs.Empty);
                    return;
                default:
                    chatClient.SendMessage(HelpText);
                    return;
            }
        }

        private void AddWord(IChatClient chatClient, string word, string newAlias, string typeName)
        {
            var existingWord = _repository.Single(CommandWordPolicy.ByWord(newAlias));

            if (string.IsNullOrEmpty(newAlias))
            {
                chatClient.SendMessage(HelpText);
                return;
            }

            if (existingWord != null)
            {
                chatClient.SendMessage($"The command word '!{existingWord.CommandWord}' already exists.");
                return;
            }

            var newCommand = new CommandWordEntity
            {
                CommandWord = newAlias,
                FullTypeName = typeName,
                IsPrimary = false
            };

            _repository.Create(newCommand);

            chatClient.SendMessage($"Created new command alias '!{newAlias}' for '!{word}'.");
        }

        private void DeleteWord(IChatClient chatClient, string word)
        {
            var existingWord = _repository.Single(CommandWordPolicy.ByWord(word));

            if (existingWord == null)
            {
                chatClient.SendMessage($"The command word '!{word}' doesn't exist.");
                return;
            }

            if (existingWord.IsPrimary)
            {
                chatClient.SendMessage("The primary command cannot be deleted.");
                return;
            }

            _repository.Remove(existingWord);

            chatClient.SendMessage($"The command '!{existingWord.CommandWord}' has been deleted.");
        }
    }
}