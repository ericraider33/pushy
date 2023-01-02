using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Pushy.Services;
using PushyCommon;

namespace Pushy.Hubs
{
    public class ChatHub : Hub
    {
        private const String CLIENT_INFO_KEY = "ClientInfo";
        private readonly IConnectedClients clientPool;

        public ChatHub(IConnectedClients clientPool)
        {
            this.clientPool = clientPool;
        }

        public async Task SendMessage(string user, string message)
        {
            ClientInfo info = (ClientInfo)Context.Items[CLIENT_INFO_KEY];
            info.count += 1;
            
            LoginCookieInfo userInfo = LoginCookieInfo.getLoginCookieInfo(Context.User);
            user = userInfo?.userName ?? "NOT_LOGGED_IN";
            
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