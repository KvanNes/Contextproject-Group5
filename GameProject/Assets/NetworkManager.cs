// Gebaseerd op: http://cgcookie.com/unity/2011/12/20/introduction-to-networking-in-unity/

using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using System;
using AssemblyCSharp;

public class NetworkManager : MonoBehaviour
{

    public static bool refreshing;
    public static HostData[] hostData;

    public static string gameName = "CGCookie_DuoDrive";
    public GameObject playerPrefab = null;
    public Transform spawnObject = null;

    private static CompositeKeyedDictionary<Type, int, NetworkPlayer> availableJobs
        = new CompositeKeyedDictionary<Type, int, NetworkPlayer>();

    void Start() {
        if (MainScript.networkManager != null) {
            throw new UnityException("NetworkManager is being created twice, it seems");
        }

        MainScript.networkManager = this;
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
        Type type = (typeString == "throttler" ? typeof(Throttler) : typeof(Driver));
        NetworkPlayer currentPlayer = availableJobs.Get(type, carNumber);

        if (currentPlayer == default(NetworkPlayer)) {
            availableJobs.Set(type, carNumber, player);
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
        this.networkView.RPC("chooseJob", RPCMode.Server, pendingType, pendingCarNumber);
    }

    public void OnDisconnectedFromServer() {
        foreach(GameObject go in GameObject.FindGameObjectsWithTag("Player")) {
            Destroy(go);
        }
        DuoDriveGUI.connected = false;
        MainScript.selfCar = null;
        MainScript.selfPlayer = null;
        MainScript.selfType = MainScript.PlayerType.None;
        MainScript.selectionIsFinal = false;
        refreshing = true;
    }

    [RPC]
    public void chooseJob(string typeString, int carNumber, NetworkMessageInfo info) {
        bool ok = checkJobAvailableAndMaybeAdd(typeString, carNumber, info.sender);
        if (ok) {
            this.networkView.RPC("chooseJobAvailable", info.sender);
        } else {
            this.networkView.RPC("chooseJobNotAvailable", info.sender);
        }
    }

    [RPC]
    public void chooseJobAvailable() {
        MainScript.selectionIsFinal = true;
    }
    
    [RPC]
    public void chooseJobNotAvailable() {
        Network.Disconnect();
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
    }

    public void Update() {
        if (refreshing && Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork) {
            if (MasterServer.PollHostList().Length > 0) {
                refreshing = false;
                hostData = MasterServer.PollHostList();
            }
        }
    }

    public void spawnPlayer(int position) {
        float y = 0.07f - 0.05f * position;
        Vector3 pos = spawnObject.position + new Vector3(0, y, 0);

        UnityEngine.Object car = Network.Instantiate(playerPrefab, pos, Quaternion.identity, 0);
        AutoBehaviour ab = (AutoBehaviour) ((GameObject) car).GetComponent(typeof(AutoBehaviour));
        ab.setCarNumber(position);
        ab.networkView.RPC("setCarNumber", RPCMode.OthersBuffered, position);
    }

    public void OnServerInitialized() {
        for (int i = 0; i < GameData.CARS_AMOUNT; i++) {
            spawnPlayer(i);
        }
    }

    public void OnPlayerConnected(NetworkPlayer player) {

    }

    public void OnPlayerDisconnected(NetworkPlayer player) {
        Network.RemoveRPCs(player);
        Network.DestroyPlayerObjects(player);

        availableJobs.Remove(player);
    }

    public NetworkPlayer[] getConnections() {
        return Network.connections;
    }

}
