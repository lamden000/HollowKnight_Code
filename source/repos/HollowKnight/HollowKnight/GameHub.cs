namespace HollowKnight
{
    using Microsoft.AspNetCore.SignalR;
    using System.Threading.Tasks;

    public class GameHub : Hub
    {
        public async Task SendPlayerPosition(string playerId, float x, float y)
        {
            await Clients.Others.SendAsync("ReceivePlayerPosition", playerId, x, y);
        }
    }
}
