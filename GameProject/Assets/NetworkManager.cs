// Gebaseerd op: http://cgcookie.com/unity/2011/12/20/introduction-to-networking-in-unity/

using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using System;

public class NetworkManager : MonoBehaviour
{

    public static bool refreshing;
    public static HostData[] hostData;

    public static string gameName = "CGCookie_DuoDrive";
    public GameObject playerPrefab = null;
    public Transform spawnObject = null;

    public static NetworkManager instance;
    public static NetworkView networkViewInstance;

    public static string type = "";
    public static int car = -1;
    private static Dictionary<string, Dictionary<int, NetworkPlayer>> availableJobs = new Dictionary<string, Dictionary<int, NetworkPlayer>>();
    
    private static T getDictionaryValue<S, T>(Dictionary<S, T> dictionary, S key) {
        T result;
        try {
            dictionary.TryGetValue(key, out result);
        } catch(Exception) {
            return default(T);
        }
        return result;
    }


    void Start() {
        Application.runInBackground = true;

        if (instance == null) {
            instance = this;
            networkViewInstance = networkView;
        } else {
            throw new UnityException("NetworkManager is being created twice, it seems");
        }

        Dictionary<int, NetworkPlayer> steererDictionary = new Dictionary<int, NetworkPlayer>();
        Dictionary<int, NetworkPlayer> throttlerDictionary = new Dictionary<int, NetworkPlayer>();
        availableJobs.Add("steerer", steererDictionary);
        availableJobs.Add("throttler", throttlerDictionary);
    }

    /**
     * Start a server with a special port assigned.
     */
    public static void startServer() {
        Network.InitializeServer(32, GameData.PORT, !Network.HavePublicAddress());
        MasterServer.RegisterHost(gameName, "2P1C", "Join this room!");
    }

    [RPC]
    public bool checkJobAvailableAndMaybeAdd(string typeString, int carNumber, NetworkPlayer player) {
        Dictionary<int, NetworkPlayer> subDirectory = getDictionaryValue(availableJobs, typeString);
        if (subDirectory == null) {
            return false;
        }

        NetworkPlayer currentPlayer = getDictionaryValue(subDirectory, carNumber);
        if (currentPlayer == default(NetworkPlayer)) {
            subDirectory.Remove(carNumber);
            subDirectory.Add(carNumber, player);
            return true;
        } else {
            return false;
        }
    }

    private static string pendingType = "";
    private static int pendingCarNumber = -1;
    public static void chooseJobFromGUI(string typeString, int carNumber) {
        pendingType = typeString;
        pendingCarNumber = carNumber;
    }

    public void OnConnectedToServer() {
        networkViewInstance.RPC("chooseJob", RPCMode.Server, pendingType, pendingCarNumber);
    }

    public void OnDisconnectedFromServer() {
        foreach(GameObject go in GameObject.FindGameObjectsWithTag("Player")) {
            Destroy(go);
        }
        DuoDriveGUI.connected = false;
        refreshing = true;
    }

    [RPC]
    public void chooseJob(string typeString, int carNumber, NetworkMessageInfo info) {
        bool ok = checkJobAvailableAndMaybeAdd(typeString, carNumber, info.sender);
        networkViewInstance.RPC("chooseJobOK", info.sender, ok, typeString, carNumber);
    }

    [RPC]
    public void chooseJobOK(bool ok, string typeString, int carNumber) {
        if (ok) {
            type = typeString;
            car = carNumber;
        } else {
            Network.Disconnect();
        }
    }

    /**
     * Refresh the hosts that are (or are not) available.
     */
    public static void refreshHostList() {
        MasterServer.RequestHostList(gameName);
        refreshing = true;
    }

    /**
     * Method to make a connection with the server.
     */
    public static void Connect(HostData host_data) {
        Network.Connect(host_data);
        Debug.Log("Connected");
    }

    public void Update() {
        if (refreshing && Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork) {
            if (MasterServer.PollHostList().Length > 0) {
                refreshing = false;
                hostData = MasterServer.PollHostList();
            }
        }
    }

    public void spawnPlayer(float y, int number) {
        Vector3 pos = spawnObject.position + new Vector3(1f, y, 0);
        UnityEngine.Object car = Network.Instantiate(playerPrefab, pos, Quaternion.identity, 0);
        AutoBehaviour ab = (AutoBehaviour) ((GameObject) car).GetComponent(typeof(AutoBehaviour));
        ab.carNumber = number;
        ab.networkView.RPC("setCarNumber", RPCMode.OthersBuffered, number);
    }

    [RPC]
    public void setAvailablePosition(int position) {
        spawnPlayer(0.07f - 0.05f * position, position);
    }

    public void OnServerInitialized() {
        Debug.Log("Server Initialized!");
        setAvailablePosition(0);
        setAvailablePosition(1);
    }

    public void OnPlayerConnected(NetworkPlayer player) {
        networkViewInstance.RPC(GameData.MESSAGE_PLAYER_CONNECTED, RPCMode.Server, player.guid);
        networkViewInstance.RPC(GameData.MESSAGE_AMOUNT_PLAYERS, RPCMode.Server, "connected");
    }

    public void OnPlayerDisconnected(NetworkPlayer player) {
        Network.RemoveRPCs(player);
        Network.DestroyPlayerObjects(player);
        networkViewInstance.RPC(GameData.MESSAGE_AMOUNT_PLAYERS, RPCMode.Server, "disconnected");

        foreach (KeyValuePair<string, Dictionary<int, NetworkPlayer>> pair in availableJobs) {
            foreach(KeyValuePair<int, NetworkPlayer> subPair in pair.Value) {
                if(subPair.Value == player) {
                    pair.Value.Remove(subPair.Key);
                    return;
                }
            }
        }
    }

    public void OnMasterServerEvent(MasterServerEvent mse) {
        if (mse == MasterServerEvent.RegistrationSucceeded) {
            Debug.Log("Registered Server!");
        }
    }

    public NetworkPlayer[] getConnections() {
        return Network.connections;
    }

    [RPC]
    public void SendInfoToServer(NetworkPlayer player) {
        string someInfo = "Client " + player.guid + ": hello server";
        networkViewInstance.RPC("ReceiveInfoFromClient", RPCMode.Server, someInfo);
    }

    [RPC]
    public void ReceiveInfoFromClient(string someInfo) {
        Debug.Log(someInfo);
    }


    // SERVER PROMPT METHODS
    [RPC]
    public void PlayerConnected(string Info) {
        string Message = "A new player has just connected. The GUID: " + Info;
        Debug.Log(Message);
    }

    [RPC]
    public void AmountPlayersConnected(string Info) {
        string Message = "A player has just " + Info + ". The new amount of players is: " + getConnections().Length;
        Debug.Log(Message);
    }

}
