namespace DevChatter.Bot.Games.Mud.Things
{
    public class Tool : IThing, IHoldable
    {
        public Tool(string name)
        {
            Name = name;
        }

        public string Name { get; }
    }
}
