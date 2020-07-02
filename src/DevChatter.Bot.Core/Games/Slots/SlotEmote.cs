namespace DevChatter.Bot.Core.Games.Slots
{
    public class SlotEmote
    {
        public string Text { get; set; }
        public int TriplePayout { get; set; }
        public int DoublePayout { get; set; }
        public int SinglePayout { get; set; }

        public SlotEmote()
        {
        }

        public SlotEmote(string text, int triplePayout, int doublePayout, int singlePayout)
        {
            Text = text;
            TriplePayout = triplePayout;
            DoublePayout = doublePayout;
            SinglePayout = singlePayout;
        }
    }
}
