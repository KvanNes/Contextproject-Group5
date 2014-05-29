using UnityEngine;
using System.Collections;

public interface INetwork
{
    object Instantiate(UnityEngine.Object prefab, Vector3 location, Quaternion rotation, int group);
    NetworkConnectionError InitializeServer(int maxConnection, int portnumber, bool NATPunchthrough);
    void Disconnect();
}