using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DevChatter.Bot.Core.Util.FuzzyMatching;

namespace DevChatter.Bot.Core.Commands.Trackers
{
    public class CommandList : IEnumerable<IBotCommand>
    {
        private readonly IList<IBotCommand> _list;
        private IBkTree<string, (IBotCommand command, IList<string> arguments)> BkTree { get; }

        private int FuzzySearchMaxDistance { get; }

        public CommandList(IList<IBotCommand> list, int fuzzySearchMaxDistance = 2)
        {
            _list = list ?? throw new ArgumentNullException(nameof(list));

            FuzzySearchMaxDistance = fuzzySearchMaxDistance;

            var bkTree =
                new MutableBkTree<string, (IBotCommand command, IList<string> arguments)>(new CaseInsensitiveMetric(new DamerauLevenshteinMetric()));
            bkTree.AddAll(_list.SelectMany(command => command.CommandWords.Select(word => (word.Word, (command, word.Args)))));
            BkTree = bkTree;
        }

        public T GetCommandByType<T>() where T : class, IBotCommand
        {
            return _list.OfType<T>().SingleOrDefault();
        }

        public IEnumerator<IBotCommand> GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable) _list).GetEnumerator();
        }

        public IBotCommand GetCommandByFullTypeName(string fullTypeName)
        {
            return _list.SingleOrDefault(x => x.GetType().FullName == fullTypeName);
        }

        public IBotCommand FindCommandByKeyword(string keyword, out IList<string> args)
        {
            var searcher = new BkTreeSearcher<string, (IBotCommand command, IList<string> arguments)>(BkTree);

            var command = searcher
                .Search(keyword, FuzzySearchMaxDistance)
                .FirstOrDefault(it => it.MatchValue.command.IsEnabled);

            if (command == null)
            {
                args = new List<string>();
                return null;
            }

            args = command.MatchValue.arguments ?? new List<string>();

            return command.MatchValue.command;
        }
    }
}
