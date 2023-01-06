using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Pushy.Services;
using PushyCommon;

namespace Pushy.Hubs
{
    [Authorize]
    public class ChatHub : Hub
    {
        private const String CLIENT_INFO_KEY = "ClientInfo";
        private readonly IConnectedClients clientPool;
        private readonly ICreateTokenService tokenService;

        public ChatHub(IConnectedClients clientPool, ICreateTokenService tokenService)
        {
            this.clientPool = clientPool;
            this.tokenService = tokenService;
        }

        public async Task SendMessage(string user, string message)
        {
            ClientInfo info = (ClientInfo)Context.Items[CLIENT_INFO_KEY];
            info.count += 1;
            
            UserInfo userInfo = tokenService.getUserInfo(Context.User);
            user = userInfo?.UserName ?? "NOT_LOGGED_IN";
            
            await Clients.All.SendAsync("ReceiveMessage", user, $"Count={info.count} message={message}");
        }
        
        public override Task OnConnectedAsync()
        {
            ClientInfo info = clientPool.addClient(Context.ConnectionId);
            if (info != null)
                Context.Items.Add(CLIENT_INFO_KEY, info);
            
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            clientPool.removeClient(Context.ConnectionId);
            return base.OnDisconnectedAsync(exception);
        }        
    }
}