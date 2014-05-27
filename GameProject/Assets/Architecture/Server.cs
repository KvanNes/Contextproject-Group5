using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using System;

public class Server : MonoBehaviour {
    public Game Game { get; set; }

    public GameObject playerPrefab = null;
    public Transform spawnObject = null;

    public void startServer() {
        Network.InitializeServer(32, GameData.PORT, !Network.HavePublicAddress());
        MasterServer.RegisterHost(GameData.GAME_NAME, "2P1C");
    }
    
    [RPC]
    public bool checkJobAvailableAndMaybeAdd(string typeString, int carNumber, NetworkPlayer networkPlayer) {
        if (carNumber < 0 || carNumber >= this.Game.Cars.Count) {
            return false;
        }

        Car car = this.Game.Cars[carNumber];
        if (car == null) {
            return false;
        }

        Player player = (typeString == "Throttler" ? car.Throttler : car.Driver);
        if(player.NetworkPlayer != default(NetworkPlayer)) {
            return false;
        }

        if (typeString == "Throttler") {
            car.Throttler.NetworkPlayer = networkPlayer;
        } else {
            car.Driver.NetworkPlayer = networkPlayer;
        }
        return true;
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
    
    private void spawnPlayer(int position) {
        float y = 0.07f - 0.05f * position;
        Vector3 pos = spawnObject.position + new Vector3(0, y, 0);
        
        UnityEngine.Object obj = Network.Instantiate(playerPrefab, pos, Quaternion.identity, 0);
        AutoBehaviour ab = (AutoBehaviour) ((GameObject) obj).GetComponent(typeof(AutoBehaviour));
        Car car = new Car(ab);
        car.Throttler = new Player(car, new Throttler());
        car.Driver = new Player(car, new Driver());
        this.Game.addCar(car);
        ab.networkView.RPC("setCarNumber", RPCMode.OthersBuffered, position);
    }
    
    private void OnServerInitialized() {
        this.Game = new Game();
        Camera.main.transform.position = new Vector3(0, 0, 10);
        for (int i = 0; i < GameData.CARS_AMOUNT; i++) {
            spawnPlayer(i);
        }
    }

    private void OnGUI() {
        if (this.Game != null) {
            // Gebaseerd op: http://answers.unity3d.com/questions/296204/gui-font-size.html
            GUI.skin.label.fontSize = 20;

            GUI.Label(new Rect(10, 10, 200, 50), new GUIContent("Server started"));
        }
    }
    
    private void OnPlayerConnected(NetworkPlayer player) {
        
    }
    
    private void OnPlayerDisconnected(NetworkPlayer player) {
        Network.RemoveRPCs(player);
        Network.DestroyPlayerObjects(player);
        
        foreach (Car car in this.Game.Cars) {
            if (car.Driver.NetworkPlayer == player) {
                car.Driver.NetworkPlayer = default(NetworkPlayer);
                break;
            } else if (car.Throttler.NetworkPlayer == player) {
                car.Throttler.NetworkPlayer = default(NetworkPlayer);
                break;
            }
        }
    }
}
