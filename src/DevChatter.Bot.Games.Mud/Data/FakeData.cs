using System.Collections.Generic;
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
            var mainHall = new Room
            {
            };
            var entranceRoom = new Room
            {
                Items = new List<Item>(),
                NorthRoom = mainHall
            };
            var item = new Item { InRoom = entranceRoom, ItemType = "Torch"};
            entranceRoom.Items.Add(item);

            return entranceRoom;
        }
    }
}
