using UnityEngine;

namespace Interfaces
{
    public interface INetwork
    {
        Object Instantiate(Object prefab, Vector3 location, Quaternion rotation, int group);
        NetworkConnectionError InitializeServer(int maxConnection, int portnumber, bool natPunchthrough);
        void RemoveRPCs(NetworkPlayer networkPlayer);
        void DestroyPlayerObjects(NetworkPlayer networkPlayer);
        void Disconnect();
        bool IsServer();
    }
}