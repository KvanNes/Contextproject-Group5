using UnityEngine;

namespace Mock
{
    public interface INetwork
    {
        Object Instantiate(Object prefab, Vector3 location, Quaternion rotation, int group);
        NetworkConnectionError InitializeServer(int maxConnection, int portnumber, bool natPunchthrough);
        void Disconnect();
    }
}