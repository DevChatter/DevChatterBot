namespace DevChatter.Bot.Core.Data.Model
{
    public class CommandSettingsEntity : DataEntity
    {
        public string SettingsTypeName { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
