using System;
using System.Collections;
using System.Collections.Generic;

namespace DevChatter.Bot.Core.Commands
{
    public class CommandList : IList<IBotCommand>
    {
        private readonly IList<IBotCommand> _list;

        public CommandList(IList<IBotCommand> list)
        {
            _list = list ?? throw new ArgumentNullException(nameof(list));
        }

        public IEnumerator<IBotCommand> GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable) _list).GetEnumerator();
        }

        public void Add(IBotCommand item)
        {
            _list.Add(item);
        }

        public void Clear()
        {
            _list.Clear();
        }

        public bool Contains(IBotCommand item)
        {
            return _list.Contains(item);
        }

        public void CopyTo(IBotCommand[] array, int arrayIndex)
        {
            _list.CopyTo(array, arrayIndex);
        }

        public bool Remove(IBotCommand item)
        {
            return _list.Remove(item);
        }

        public int Count => _list.Count;

        public bool IsReadOnly => _list.IsReadOnly;

        public int IndexOf(IBotCommand item)
        {
            return _list.IndexOf(item);
        }

        public void Insert(int index, IBotCommand item)
        {
            _list.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            _list.RemoveAt(index);
        }

        public IBotCommand this[int index]
        {
            get => _list[index];
            set => _list[index] = value;
        }
    }
}