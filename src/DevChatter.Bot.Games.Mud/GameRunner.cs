using DevChatter.Bot.Core.Systems.Chat;
using DevChatter.Bot.Games.Mud.Locations;

namespace DevChatter.Bot.Games.Mud
{
    public class GameRunner
    {
        private readonly IMessageSender _messageSender;

        public GameRunner(IMessageSender messageSender)
        {
            _messageSender = messageSender;
        }

        public CharacterInfo CharacterInfo { get; set; }
        public Room CurrentRoom { get; set; }

        public void Run()
        {
            DescribeState();
        }

        private void DescribeState()
        {
            _messageSender.SendMessage("");
        }
    }
}
