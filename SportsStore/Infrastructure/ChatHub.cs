using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsStore.Infrastructure
{
    public class ChatHub : Hub
    {
        public async Task Send(string message, string userName)
        {
            await this.Clients.All.SendAsync("Send", message, userName);
        }

        public override async Task OnConnectedAsync()
        {
            await Clients.All.SendAsync("OnConnectedAsync", $"{Context.ConnectionId}");
            await base.OnConnectedAsync();
        }
        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await Clients.All.SendAsync("OnDisconnectedAsync", $"{Context.ConnectionId}");
            await base.OnDisconnectedAsync(exception);
        }
    }
}
