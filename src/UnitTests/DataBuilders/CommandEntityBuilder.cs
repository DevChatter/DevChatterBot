using System.Collections.Generic;
using DevChatter.Bot.Core.Data.Model;

namespace UnitTests.DataBuilders
{
    public class CommandEntityBuilder
    {
        private CommandEntity _entity = new CommandEntity();

        public CommandEntityBuilder CommandWord(string commandWord)
        {
            _entity.CommandWord = commandWord;
            return this;
        }

        public CommandEntityBuilder AddAlias(string alias)
        {
            _entity.Aliases.Add(new AliasEntity { Word = alias });
            return this;
        }

        public CommandEntityBuilder ClearAliases()
        {
            _entity.Aliases.Clear();
            return this;
        }

        public CommandEntityBuilder WithDefaults()
        {
            _entity = new CommandEntity
            {
                CommandWord = "FizzBuzz",
                Aliases = new List<AliasEntity>
                {
                    new AliasEntity {Word = "Fizz"},
                    new AliasEntity {Word = "Buzz"},
                }
            };
            return this;
        }

        public CommandEntity Build()
        {
            var built = _entity;
            _entity = new CommandEntity();
            return built;
        }
    }
}
