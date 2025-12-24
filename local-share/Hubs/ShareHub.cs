using local_share.Models;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Concurrent;

namespace local_share.Hubs
{
    public class ShareHub : Hub
    {
        private static readonly ConcurrentDictionary<string, OnlineUser> _users = new();

        public Task Alive() => Task.CompletedTask;

        public async Task SendMessage(string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", message);
        }

        public override async Task OnConnectedAsync()
        {
            var user = new OnlineUser
            {
                ConnectionId = Context.ConnectionId,
                Name = Context.GetHttpContext()?.Request.Query["name"].ToString()
            };

            _users[Context.ConnectionId] = user;

            await Clients.All.SendAsync("UsersUpdated", _users.Values);

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            _users.TryRemove(Context.ConnectionId, out _);

            await Clients.All.SendAsync("UsersUpdated", _users.Values);

            await base.OnDisconnectedAsync(exception);
        }

    }
}
