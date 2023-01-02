using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Pushy.Hubs;

namespace Pushy.Services
{
    public class ChatService : IPollActionAsync
    {
        private readonly IHubContext<ChatHub> hubContext;
        private readonly IConnectedClients pool;

        public ChatService(IHubContext<ChatHub> hubContext, IConnectedClients pool)
        {
            this.hubContext = hubContext;
            this.pool = pool;
        }

        public async Task poll()
        {
            String message = $"Time is {DateTime.Now} with pool of {pool.size}";
            await hubContext.Clients.All.SendAsync("ReceiveMessage", message); 
            
            Console.WriteLine($"{DateTime.Now} - Sent to=All message={message}");
        }
    }
}