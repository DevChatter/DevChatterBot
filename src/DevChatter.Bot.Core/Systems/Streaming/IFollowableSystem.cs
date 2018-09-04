using System.Threading.Tasks;

namespace DevChatter.Bot.Core.Systems.Streaming
{
    public interface IFollowableSystem
    {
        Task Connect();
        Task Disconnect();
    }
}
