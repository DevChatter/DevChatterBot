using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using Microsoft.Extensions.DependencyInjection;

namespace DevChatter.Bot.Core.Commands.Trackers
{
    public class CommandList : IEnumerable<IBotCommand>
    {
        private readonly IComponentContext _serviceProvider;

        public void Refresh()
        {
            List<IBotCommand> botCommands =
                _serviceProvider.Resolve<IList<IBotCommand>>().ToList();
            _list.Clear();
            foreach (IBotCommand botCommand in botCommands)
            {
                _list.Add(botCommand);
            }
        }

        private readonly IList<IBotCommand> _list;

        public CommandList(IList<IBotCommand> list, IComponentContext serviceProvider)
        {
            _list = list ?? throw new ArgumentNullException(nameof(list));
            _serviceProvider = serviceProvider;
        }

        public IEnumerator<IBotCommand> GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable) _list).GetEnumerator();
        }
    }
}
