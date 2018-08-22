using DevChatter.Bot.Games.Mud.Data.Model;

namespace DevChatter.Bot.Games.Mud.Data
{
    public static class FakeData
    {
        /// <summary>
        /// Gets a dungeon all based on a starting room.
        /// </summary>
        /// <returns>Starting Room</returns>
        public static Room GetDungeon()
        {
            return new Room();
        }
    }
}
