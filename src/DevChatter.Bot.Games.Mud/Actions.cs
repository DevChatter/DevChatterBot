using System.Collections.Generic;
using System.Linq;

namespace DevChatter.Bot.Games.Mud
{
    public class Actions
    {
        public static Actions Look = new Actions(1, nameof(Look), "check", "look at", "examine");
        public static Actions Take = new Actions(2, nameof(Take), "get", "pick up", "grab");
        public static Actions Drop = new Actions(3, nameof(Drop), "discard", "trash", "leave");
        public static Actions Attack = new Actions(4, nameof(Attack), "fight");
        public static Actions Use = new Actions(5, nameof(Use));
        public static Actions Hide = new Actions(6, nameof(Hide));

        private Actions(int id, params string[] aliases)
        {
            Id = id;
            _aliases = new List<string>(aliases);
        }

        public int Id { get; }
        private readonly List<string> _aliases;

        public string PrimaryWord => _aliases.First();

        public bool IsMatch(string verb)
        {
            return _aliases.Contains(verb);
        }
    }
}
