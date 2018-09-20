namespace DevChatter.Bot.Core.Data.Model
{
    public class AliasArgumentEntity : DataEntity
    {
        public CommandEntity CommandEntity { get; set; }
        public int Index { get; set; }
        public string Argument { get; set; }
    }
}
