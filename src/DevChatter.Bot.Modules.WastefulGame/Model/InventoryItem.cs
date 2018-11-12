namespace DevChatter.Bot.Modules.WastefulGame.Model
{
    public class InventoryItem : GameData
    {
        public Survivor Survivor { get; set; }
        public string Name { get; set; }
        public int Uses { get; set; }
    }
}
