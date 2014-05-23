// Gebaseerd op: http://cgcookie.com/unity/2011/12/20/introduction-to-networking-in-unity/

using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

public class NetworkManager : MonoBehaviour
{

    public static bool refreshing;
    public static HostData[] hostData;

    public static string gameName = "CGCookie_DuoDrive";
    public GameObject playerPrefab = null;
    public Transform spawnObject = null;

    List<bool> beschikbaar = new List<bool> { false, true };
    Dictionary<NetworkPlayer, int> beschikbaarWie = new Dictionary<NetworkPlayer, int>();

    void Start()
    {
        Application.runInBackground = true;
    }

    /**
     * Start a server with a special port assigned.
     */
    public static void startServer()
    {
        Network.InitializeServer(32, GameData.PORT, !Network.HavePublicAddress());
        MasterServer.RegisterHost(gameName, "Duo Drive", "Join this room!");
    }

    /**
     * Refresh the hosts that are (or are not) available.
     */
    public static void refreshHostList()
    {
        MasterServer.RequestHostList(gameName);
        refreshing = true;
    }

    /**
     * Method to make a connection with the server.
     */
    public static void Connect(HostData host_data)
    {
        Network.Connect(host_data);
        Debug.Log("Connected");
    }

    void Update()
    {
        if (refreshing)
        {
            if (MasterServer.PollHostList().Length > 0)
            {
                refreshing = false;
                hostData = MasterServer.PollHostList();
            }
        }
    }

    void spawnPlayer(float y)
    {
        Vector3 pos = spawnObject.position + new Vector3(1f, y, 0);
        Network.Instantiate(playerPrefab, pos, Quaternion.identity, 0);
    }

    [RPC]
    void setAvailablePosition(int position)
    {
        if (position != -1)
        {
            spawnPlayer(0.07f - 0.05f * position);
        }
        else
        {
            GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("Player");
            foreach (GameObject obj in gameObjects)
            {
                Destroy(obj);
            }
            Network.Disconnect();
        }
    }

    void OnServerInitialized()
    {
        Debug.Log("Server Initialized!");
        setAvailablePosition(0);
    }

    void OnPlayerConnected(NetworkPlayer player)
    {
        networkView.RPC("setAvailablePosition", player, availablePosition(player));
        networkView.RPC(GameData.MESSAGE_PLAYER_CONNECTED, RPCMode.Server, player.guid);
        networkView.RPC(GameData.MESSAGE_AMOUNT_PLAYERS, RPCMode.Server, "connected");
    }

    void OnPlayerDisconnected(NetworkPlayer player)
    {
        Network.RemoveRPCs(player);
        Network.DestroyPlayerObjects(player);
        int value;
        if (beschikbaarWie.TryGetValue(player, out value))
        {
            beschikbaar[value] = true;
            beschikbaarWie.Remove(player);
        }
        networkView.RPC(GameData.MESSAGE_AMOUNT_PLAYERS, RPCMode.Server, "disconnected");
    }

    int availablePosition(NetworkPlayer networkPlayer)
    {
        for (int i = 0; i < beschikbaar.Count; i++)
        {
            if (beschikbaar[i])
            {
                beschikbaar[i] = false;
                beschikbaarWie.Add(networkPlayer, i);
                return i;
            }
        }
        return -1;
    }

    void OnMasterServerEvent(MasterServerEvent mse)
    {
        if (mse == MasterServerEvent.RegistrationSucceeded)
        {
            Debug.Log("Registered Server!");
        }
    }

    public NetworkPlayer[] getConnections()
    {
        return Network.connections;
    }

    [RPC]
    void SendInfoToServer(NetworkPlayer player)
    {
        string someInfo = "Client " + player.guid + ": hello server";
        networkView.RPC("ReceiveInfoFromClient", RPCMode.Server, someInfo);
    }

    [RPC]
    void ReceiveInfoFromClient(string someInfo)
    {
        Debug.Log(someInfo);
    }


    // SERVER PROMPT METHODS
    [RPC]
    void PlayerConnected(string Info)
    {
        string Message = "A new player has just connected. The GUID: " + Info;
        Debug.Log(Message);
    }

    [RPC]
    void AmountPlayersConnected(string Info)
    {
        string Message = "A player has just " + Info + ". The new amount of players is: " + getConnections().Length;
        Debug.Log(Message);
    }

}
