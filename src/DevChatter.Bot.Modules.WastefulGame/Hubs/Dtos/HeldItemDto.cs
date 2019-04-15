using DevChatter.Bot.Modules.WastefulGame.Model;

namespace DevChatter.Bot.Modules.WastefulGame.Hubs.Dtos
{
    public class HeldItemDto
    {
        public string Name { get; set; }
        public int Uses { get; set; }

        public InventoryItem ToInventoryItem()
        {
            return new InventoryItem
            {
                Name = Name,
                Uses = Uses
            };
        }

        public static HeldItemDto FromInventoryItem(InventoryItem inventoryItem)
        {
            return new HeldItemDto
            {
                Name = inventoryItem.Name,
                Uses = inventoryItem.Uses
            };
        }
    }
}
