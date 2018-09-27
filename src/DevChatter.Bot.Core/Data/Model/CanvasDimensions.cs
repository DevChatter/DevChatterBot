namespace DevChatter.Bot.Core.Data.Model
{
    public class CanvasProperties : DataEntity
    {
        public string CanvasId { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int TopY { get; set; }
        public int LeftX { get; set; }
    }
}
