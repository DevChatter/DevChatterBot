using System.Collections.Generic;
using System.Linq;
using DevChatter.Bot.Core.Util;
using HR = DevChatter.Bot.Core.Games.Heist.HeistRoles;

namespace DevChatter.Bot.Core.Games.Heist
{
    public class HeistMission
    {
        private readonly int _id;

        internal static readonly List<HeistMission> Missions = new List<HeistMission>
        {
            new HeistMission(1,"Bank Robbery", new []{HR.Thief, HR.Tech, HR.Grifter, HR.Driver, HR.Carrier}, 1000),
            new HeistMission(2,"Mob Bank Robbery", new []{HR.Thief, HR.Tech, HR.Grifter,  HR.Hitter, HR.Driver, HR.Carrier}, 1500),
            new HeistMission(3,"Museum Heist", new []{HR.Thief, HR.Grifter, HR.Hacker, HR.Driver, HR.Carrier}, 1000),
            new HeistMission(4,"Jewelry Store", new []{HR.Grifter, HR.Hacker, HR.Driver, HR.Carrier}, 800),
        };

        public static HeistMission GetById(int id) => Missions.SingleOrDefault(x => x._id == id);

        public HeistMission(int id, string name, IEnumerable<HeistRoles> preferredRoles, int reward)
        {
            _id = id;
            Name = name;
            PreferredRoles.AddRange(preferredRoles);
            Reward = reward;
        }

        public string Name { get; }
        public List<HeistRoles> PreferredRoles { get; } = new List<HeistRoles>();
        public int Reward { get; }

        public HeistMissionResult AttemptHeist(Dictionary<HeistRoles, string> heistMembers)
        {
            int randomNumber = MyRandom.RandomNumber(0,100);

            var partyStrength = GetPartyStrength(heistMembers.Keys);
            var heistMissionResult = new HeistMissionResult();
            if (randomNumber < partyStrength)
            {
                heistMissionResult.ResultMessages.Add($"Somehow, you all managed to complete the {Name}. I wish the system gave you your prize of {Reward} tokens, but it doesn't yet.");
                heistMissionResult.SurvivingMembers.AddRange(heistMembers.Values);
            }
            else
            {
                heistMissionResult.ResultMessages.Add("Everyone got arrested, because they talked about the heist publicly on a Twitch chat... And then they waited 2 minutes, giving the cops time to catch them. Fools!");
            }
            return heistMissionResult;

        }

        private int GetPartyStrength(IEnumerable<HeistRoles> heistRoles)
        {
            int strengthTotal = 0;

            foreach (HeistRoles heistRole in heistRoles)
            {
                strengthTotal += PreferredRoles.Contains(heistRole) ? 10 : 5;
            }

            return strengthTotal;
        }
    }

    public class HeistMissionResult
    {
        public List<string> ResultMessages { get; set; } = new List<string>();
        public List<string> SurvivingMembers { get; set; } = new List<string>();
    }
}
