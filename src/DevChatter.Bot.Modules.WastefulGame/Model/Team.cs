using System.Collections.Generic;

namespace DevChatter.Bot.Modules.WastefulGame.Model
{
    public class Team : GameData
    {
        public const int JOIN_TEAM_COST = 50;
        public string Name { get; protected set; }
        public List<Survivor> Members { get; protected set; } = new List<Survivor>();

        public bool Join(Survivor survivor)
        {
            if (survivor.Team == null && survivor.TryPay(JOIN_TEAM_COST))
            {
                Members.Add(survivor);
                return true;
            }

            return false;
        }
    }
}
