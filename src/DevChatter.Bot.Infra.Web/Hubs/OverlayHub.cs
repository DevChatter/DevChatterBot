using DevChatter.Bot.Core.Streaming;
using Microsoft.AspNetCore.SignalR;

namespace DevChatter.Bot.Infra.Web.Hubs
{
    public class OverlayHub : Hub<IOverlayDisplay>
    {
    }
}
