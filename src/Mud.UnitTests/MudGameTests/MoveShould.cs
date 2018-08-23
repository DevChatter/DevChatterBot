using DevChatter.Bot.Core.Data.Model;
using DevChatter.Bot.Core.Systems.Chat;
using DevChatter.Bot.Games.Mud;
using DevChatter.Bot.Games.Mud.Data.Model;
using FluentAssertions;
using Moq;
using System.Linq;
using Xunit;

namespace Mud.UnitTests.MudGameTests
{
    public class MoveShould
    {
        [Fact]
        public void ChangeRooms_AfterMoving()
        {
            var (chatUser, mock, mudGame) = SetUpTest();

            Player player = mudGame.Players.Single();
            Room startingRoom = player.InRoom;

            mudGame.Move(chatUser, new[] {"North"});
            player.InRoom.Should().NotBe(startingRoom);
        }

        [Fact]
        public void ReturnToPreviousRoom_GivenOpposingMovement()
        {
            var (chatUser, mock, mudGame) = SetUpTest();

            Player player = mudGame.Players.Single();
            Room startingRoom = player.InRoom;

            mudGame.Move(chatUser, new[] {"North"});
            mudGame.Move(chatUser, new[] {"South"});
            player.InRoom.Should().Be(startingRoom);
        }

        private static (ChatUser, Mock<IMessageSender>, MudGame) SetUpTest()
        {
            var chatUser = new ChatUser
            {
                DisplayName = "Brendan"
            };
            var mock = new Mock<IMessageSender>();
            var mudGame = new MudGame(mock.Object);
            mudGame.AttemptToJoin(chatUser);
            return (chatUser, mock, mudGame);
        }

    }
}
