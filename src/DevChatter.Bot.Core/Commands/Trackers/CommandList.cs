using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace DevChatter.Bot.Core.Commands.Trackers
{
    public class CommandList : IEnumerable<IBotCommand>
    {
        private readonly IList<IBotCommand> _list;

        public CommandList(IList<IBotCommand> list)
        {
            _list = list ?? throw new ArgumentNullException(nameof(list));
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
    }
}
