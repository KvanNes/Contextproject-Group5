using UnityEngine;

namespace Mock
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

        public void Disconnect()
        {
            Network.Disconnect();
        }
    }
}