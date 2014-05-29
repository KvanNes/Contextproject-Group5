using UnityEngine;
using System;

public class NetworkWrapper : INetwork
{

    public string IPAdress;
    public bool connected;

    public object Instantiate(UnityEngine.Object prefab, Vector3 location, Quaternion rotation, int group)
    {
        return Network.Instantiate(prefab, location, rotation, group);
    }

    public NetworkConnectionError InitializeServer(int maxConnection, int portnumber, bool NATPunchthrough)
    {
        return Network.InitializeServer(maxConnection, portnumber, NATPunchthrough);
    }

    public void Disconnect()
    {
        Network.Disconnect();
    }
}