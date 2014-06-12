using Interfaces;
using UnityEngine;

namespace Wrappers
{
    public class NetworkWrapper : INetwork
    {
        public Object Instantiate(Object prefab, Vector3 location, Quaternion rotation,
            int group)
        {
            return Network.Instantiate(prefab, location, rotation, group);
        }

        public NetworkConnectionError InitializeServer(int maxConnection, int portnumber, bool natPunchthrough)
        {
            return Network.InitializeServer(maxConnection, portnumber, natPunchthrough);
        }

        public void RemoveRPCs(NetworkPlayer networkPlayer)
        {
            Network.RemoveRPCs(networkPlayer);
        }

        public void DestroyPlayerObjects(NetworkPlayer networkPlayer)
        {
            Network.DestroyPlayerObjects(networkPlayer);
        }

        public void Disconnect()
        {
            Network.Disconnect();
        }

        public bool IsServer()
        {
            return Network.isServer;
        }
    }
}