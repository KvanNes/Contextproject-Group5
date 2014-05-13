// Gebaseerd op: http://cgcookie.com/unity/2011/12/20/introduction-to-networking-in-unity/

using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

public class NetworkManager : MonoBehaviour {
    
    private float btnX;
    private float btnY;
    private float btnH;
    private float btnW;
    
    private bool refreshing;
    private HostData[] hostData;
    
    public string gameName = "CGCookie_DuoDrive";
    public GameObject playerPrefab = null;
    public Transform spawnObject = null;
    
    void Start() {
        Application.runInBackground = true;
        btnX = Screen.width * 0.05f;
        btnY = Screen.height * 0.05f;
        btnH = Screen.width * 0.1f;
        btnW = Screen.width * 0.1f;
    }
    
    private void startServer() {
        Network.InitializeServer(32, 25001, !Network.HavePublicAddress());
        MasterServer.RegisterHost(gameName, "Duo Drive", "Join this room!");
    }
    
    private void refreshHostList() {
        MasterServer.RequestHostList(gameName);
        refreshing = true;
    }
    
    void Update() {
        if(refreshing) {
            if(MasterServer.PollHostList().Length > 0) {
                refreshing = false;
                Debug.Log(MasterServer.PollHostList().Length);
                hostData = MasterServer.PollHostList();
            }
        }
    }
    
    void spawnPlayer(float y) {
        Vector3 pos = spawnObject.position + new Vector3(1f, y, 0);
        Network.Instantiate(playerPrefab, pos, Quaternion.identity, 0);
    }

    [RPC]
    void setAvailablePosition(int position) {
        if (position != -1) {
            spawnPlayer(0.07f - 0.05f * position);
        } else {
            GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("Player");
            foreach(GameObject obj in gameObjects) {
                Destroy(obj);
            }
            Network.Disconnect();
        }
    }
    
    void OnServerInitialized() {
        Debug.Log("Server Initialized!");
        setAvailablePosition(0);
    }

    List<bool> beschikbaar = new List<bool> { false, true }; //, true, true };
    Dictionary<NetworkPlayer, int> beschikbaarWie = new Dictionary<NetworkPlayer, int>();

    void OnPlayerConnected(NetworkPlayer player) {
        networkView.RPC("setAvailablePosition", player, availablePosition(player));
    }

    void OnPlayerDisconnected(NetworkPlayer player) {
        Network.RemoveRPCs(player);
        Network.DestroyPlayerObjects(player);
        int value;
        if (beschikbaarWie.TryGetValue(player, out value)) {
            beschikbaar[value] = true;
            beschikbaarWie.Remove(player);
        }
    }

    int availablePosition(NetworkPlayer networkPlayer) {
        for (int i = 0; i < beschikbaar.Count; i++) {
            if (beschikbaar[i]) {
                beschikbaar[i] = false;
                beschikbaarWie.Add(networkPlayer, i);
                return i;
            }
        }
        return -1;
    }
    
    void OnMasterServerEvent(MasterServerEvent mse) {
        if(mse == MasterServerEvent.RegistrationSucceeded) {
            Debug.Log("Registered Server!");
        }
    }
    
    void OnGUI() {
        if(!Network.isClient && !Network.isServer) {
            if(GUI.Button( new Rect(btnX, btnY, btnW, btnH), "Start Server")) {
                Debug.Log("Starting Server");
                startServer();
            }
            if(GUI.Button( new Rect(btnX, btnY * 1.2f + btnH, btnW, btnH), "Refresh Hosts")) {
                Debug.Log("Refresh Hosts");
                refreshHostList();
            }
            if(hostData != null) {
                for(int i = 0; i < hostData.Length; i++) {
                    if(GUI.Button (new Rect(btnX * 1.5f + btnW, btnY * 1.2f + (btnH * i), btnW * 3f, btnH / 2f), hostData[i].gameName)) {
                        Network.Connect(hostData[i]);
                        Debug.Log(hostData[i]);
                    }
                }
            }
        }
    }
}
