using System;
using System.Collections.Generic;

namespace Pushy.Services
{
    public interface IConnectedClients
    {
        ClientInfo addClient(String connectionId);
        void removeClient(String connectionId);
        int size { get; }
    }
    
    public class ConnectedClients : IConnectedClients
    {
        private readonly Dictionary<String, ClientInfo> pool = new Dictionary<String, ClientInfo>();
        public int size { get; private set; }
        
        public ClientInfo addClient(String connectionId)
        {
            if (connectionId == null)
                return null;
            
            lock (pool)
            {
                if (pool.ContainsKey(connectionId))
                    return pool[connectionId];

                ClientInfo result = new ClientInfo(connectionId);
                pool.Add(connectionId, result);
                size = pool.Count;
                return result;
            }
        }

        public void removeClient(String connectionId)
        {
            if (connectionId == null)
                return;
            
            lock (pool)
            {
                if (!pool.ContainsKey(connectionId))
                    return;
            
                pool.Remove(connectionId);
                size = pool.Count;
            }
        }
    }
}