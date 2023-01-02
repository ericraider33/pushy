using System;

namespace Pushy.Services
{
    public class ClientInfo
    {
        public String userName;
        public String connectionId;
        public int count;

        public ClientInfo(string connectionId)
        {
            this.connectionId = connectionId;
        }
    }
}