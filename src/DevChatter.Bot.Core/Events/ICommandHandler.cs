using DevChatter.Bot.Core.Events.Args;

namespace DevChatter.Bot.Core.Events
{
    public interface ICommandHandler
    {
        void CommandReceivedHandler(object sender, CommandReceivedEventArgs e);
    }
}