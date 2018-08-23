using DevChatter.Bot.Core.Data.Model;
using DevChatter.Bot.Core.Systems.Chat;
using DevChatter.Bot.Games.Mud;
using Moq;
using Xunit;

namespace Mud.UnitTests.MudGameTests
{
    public class AttemptToJoinShould
    {
        [Fact]
        public void SendMessage_WhenJoining()
        {
            var (chatUser, mock, mudGame) = SetUpTest();

            mudGame.AttemptToJoin(chatUser);

            mock.Verify(x => x.SendMessage(It.IsAny<string>()));
        }

        [Fact]
        public void SendDirectMessage_WhenAlreadyJoined()
        {
            var (chatUser, mock, mudGame) = SetUpTest();

            mudGame.AttemptToJoin(chatUser);
            mudGame.AttemptToJoin(chatUser);

            mock.Verify(x => x.SendDirectMessage(chatUser.DisplayName, It.IsAny<string>()));
        }

        private static (ChatUser, Mock<IMessageSender>, MudGame) SetUpTest()
        {
            var chatUser = new ChatUser
            {
                DisplayName = "Brendan"
            };
            var mock = new Mock<IMessageSender>();
            var mudGame = new MudGame(mock.Object);
            return (chatUser, mock, mudGame);
        }

    }
}
