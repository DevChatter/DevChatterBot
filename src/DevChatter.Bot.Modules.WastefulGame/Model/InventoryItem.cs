namespace DevChatter.Bot.Modules.WastefulGame.Model
{
    public class InventoryItem : GameData
    {
        public InventoryItem()
        {
        }

        public InventoryItem(ShopItem shopItem)
        {
            Name = shopItem.Name;
            Uses = shopItem.Uses;
        }

        public Survivor Survivor { get; set; }
        public string Name { get; set; }
        public int Uses { get; set; }
    }
}
