using System;

namespace Pushy.Services
{
    public class ChatWorker : PollBackgroundService
    {
        public ChatWorker(ChatService chatService) : base(chatService, TimeSpan.FromSeconds(5))
        {
        }
    }
}