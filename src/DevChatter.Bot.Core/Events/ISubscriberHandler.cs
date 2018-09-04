using System;
using DevChatter.Bot.Core.Events.Args;

namespace DevChatter.Bot.Core.Events
{
    public interface ISubscriberHandler
    {
        event EventHandler<NewSubscriberEventArgs> OnNewSubscriber;
    }
}
