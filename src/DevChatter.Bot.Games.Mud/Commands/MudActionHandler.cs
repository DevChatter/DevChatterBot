using DevChatter.Bot.Core.Events.Args;
using DevChatter.Bot.Core.Systems.Chat;

namespace DevChatter.Bot.Games.Mud.Commands
{
    public class MudActionHandler
    {
        private readonly IMessageReceiver _messageReceiver;

        public MudActionHandler(IMessageReceiver messageReceiver)
        {
            _messageReceiver = messageReceiver;
        }

        public void StartListening()
        {
            _messageReceiver.OnMessageReceived += _messageReceiver_OnMessageReceived;
        }

        public void StopListening()
        {
            _messageReceiver.OnMessageReceived -= _messageReceiver_OnMessageReceived;
        }

        private void _messageReceiver_OnMessageReceived(object sender, MessageReceivedEventArgs e)
        {
            // TODO: Verify in the MUD room

            // TODO: Parse to a valid action

            // TODO: Validate Action Possible in Current State

            // TODO: Apply Action, Displaying new State
        }
    }
}
