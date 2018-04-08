namespace DevChatter.Bot.Core.Data.Model
{
	public class CommandWordEntity : DataEntity
	{
		public string CommandWord { get; set; }
		public string FullTypeName { get; set; }
		public bool IsPrimary { get; set; }
	}
}