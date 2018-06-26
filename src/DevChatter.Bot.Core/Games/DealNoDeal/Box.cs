namespace DevChatter.Bot.Core.Games.DealNoDeal
{
    public class Box
    {
        public Box(int id, int value, string owner)
        {
            Id = id;
            TokenValue = value;
            Owner = owner;
        }

        public Box(int id, int value)
        {
            Id = id;
            TokenValue = value;
        }

        public int Id { get; set; }
        public int TokenValue { get; set; }
        public string Owner { get; set; }

        public bool SetOwner(string ownerName)
        {
            if (Owner != null)
            {
                return false;
            }

            Owner = ownerName;
            return true;
        }
    }
}
