using System.Collections.Generic;
using DevChatter.Bot.Core.Systems.Chat;

namespace DevChatter.Bot.Core.Games.Heist
{
    public class HeistMission
    {
        public HeistMission(string name, IEnumerable<HeistRoles> preferredRoles, int reward)
        {
            Name = name;
            PreferredRoles.AddRange(preferredRoles);
            Reward = reward;
        }

        public string Name { get; }
        public List<HeistRoles> PreferredRoles { get; } = new List<HeistRoles>();
        public int Reward { get; }

        public void AttemptHeist(IChatClient chatClient, Dictionary<HeistRoles, string> heistMembers)
        {
            chatClient.SendMessage("Everyone got arrested, because they talked about the heist publicly on a Twitch chat... And then they waited 2 minutes, giving the cops time to catch them. Fools!");
        }
    }
}