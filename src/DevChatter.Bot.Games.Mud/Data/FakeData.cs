using System.Collections.Generic;
using DevChatter.Bot.Games.Mud.Data.Model;

namespace DevChatter.Bot.Games.Mud.Data
{
    public static class FakeData
    {
        /// <summary>
        /// Gets a dungeon all based on a starting room.
        /// </summary>
        /// <returns>Starting Room with references to others</returns>
        public static Room GetDungeon()
        {
            var entranceRoom = new Room
            {
                BasicText = "a small room",
                Items = new List<Item>(),
            };
            var mainHall = new Room
            {
                BasicText = "a long, dark hallway",
                SouthRoom = entranceRoom,
            };
            entranceRoom.NorthRoom = mainHall;

            var item = new Item { InRoom = entranceRoom, ItemType = "Torch", Description = "stack of torches on the ground."};
            entranceRoom.Items.Add(item);

            return entranceRoom;
        }
    }
}
