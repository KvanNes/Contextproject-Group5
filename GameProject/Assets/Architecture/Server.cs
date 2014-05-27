using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using System;
using AssemblyCSharp;

public class Server : MonoBehaviour {
    public Game Game { get; set; }

    public GameObject playerPrefab = null;
    public Transform spawnObject = null;
    
    private CompositeKeyedDictionary<Type, int, NetworkPlayer> availableJobs
        = new CompositeKeyedDictionary<Type, int, NetworkPlayer>();

    public void startServer() {
        Network.InitializeServer(32, GameData.PORT, !Network.HavePublicAddress());
        MasterServer.RegisterHost(GameData.GAME_NAME, "2P1C");
    }
    
    [RPC]
    public bool checkJobAvailableAndMaybeAdd(string typeString, int carNumber, NetworkPlayer player) {
        Type type = (typeString == "Throttler" ? typeof(Throttler) : typeof(Driver));
        NetworkPlayer currentPlayer = availableJobs.Get(type, carNumber);
        
        if (currentPlayer == default(NetworkPlayer)) {
            availableJobs.Set(type, carNumber, player);
            return true;
        } else {
            return false;
        }
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
    
    public void spawnPlayer(int position) {
        float y = 0.07f - 0.05f * position;
        Vector3 pos = spawnObject.position + new Vector3(0, y, 0);
        
        UnityEngine.Object obj = Network.Instantiate(playerPrefab, pos, Quaternion.identity, 0);
        AutoBehaviour ab = (AutoBehaviour) ((GameObject) obj).GetComponent(typeof(AutoBehaviour));
        Car car = new Car(ab);
        this.Game.addCar(car);
        ab.networkView.RPC("setCarNumber", RPCMode.OthersBuffered, position);
    }
    
    public void OnServerInitialized() {
        this.Game = new Game();
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
}
