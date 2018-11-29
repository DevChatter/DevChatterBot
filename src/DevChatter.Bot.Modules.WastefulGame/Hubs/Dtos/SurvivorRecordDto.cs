using DevChatter.Bot.Modules.WastefulGame.Model;

namespace DevChatter.Bot.Modules.WastefulGame.Hubs.Dtos
{
    public class SurvivorRecordDto
    {
        public SurvivorRecordDto()
        {
        }

        public SurvivorRecordDto(Survivor survivor)
        {
            Name = survivor.DisplayName;
            Money = survivor.Money;
        }

        public string Name { get; set; }
        public long Money { get; set; }
    }
}
