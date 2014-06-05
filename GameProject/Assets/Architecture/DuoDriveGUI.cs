using UnityEngine;
using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;

public class DuoDriveGUI : MonoBehaviour {

    private readonly static int buttonX = 10;
    private readonly static int buttonY = 10;
    private readonly static int buttonW = 150;
    private readonly static int buttonH = 50;

    private bool ServerAvailable() {
        return NetworkController.hostData != null && NetworkController.hostData.Length > 0;
    }

    private void Start() {
        InvokeRepeating("UpdateHostList", 0f, 5f);
    }

    private void UpdateHostList() {
        if (!NetworkController.connected) {
            NetworkController.refreshHostList();
        }
    }

    private void CreateServerButton() {
        if (GUI.Button(new Rect(Screen.width / 2 - 75, Screen.height / 2 - 75, 150, 150), "Start server!")) {
            MainScript.selfType = MainScript.PlayerType.Server;
            MainScript.server.startServer();
        }
    }

    private void CreateClientButton(Type type, HostData hostData, int carNumber, int x, int y) {
        if (GUI.Button(new Rect(x, y, buttonW, buttonH), type.Name + "; Car " + carNumber.ToString())) {
            MainScript.selfType = MainScript.PlayerType.Client;
            MainScript.selfCar = new Car(carNumber);

            // Gebaseerd op: http://stackoverflow.com/a/755
            MainScript.selfPlayer = new Player(MainScript.selfCar, (PlayerRole) Activator.CreateInstance(type));
            MainScript.selfPlayer.Role.Initialize();                

            MainScript.client.chooseJobWhenConnected(type.Name, carNumber);
            Network.Connect(hostData);
            NetworkController.connected = true;
        }
    }

    private void CreateClientButtons(int startX, int startY, HostData hostData) {
        for(int j = 0; j < GameData.CARS_AMOUNT; j++) {
            int y = startY + j * buttonH;
            IEnumerable<Type> roleTypes = Utils.TypesImplementingInterface(typeof(PlayerRole));
            int roleCounter = 0;
            foreach(Type role in roleTypes) {
                CreateClientButton(role, hostData, j, startX + roleCounter, y);
                roleCounter += buttonW;
            }
        }
    }

    private void OnGUI() {
        if (NetworkController.connected || Application.internetReachability != NetworkReachability.ReachableViaLocalAreaNetwork) {
            return;
        }

        // Gebaseerd op: http://answers.unity3d.com/questions/296204/gui-font-size.html
        GUI.skin.label.fontSize = GUI.skin.button.fontSize = 20;

        if (!Network.isServer) {
            CreateServerButton();
        }

        if (ServerAvailable() && !Network.isServer && !Network.isClient) {
            int buttonY = DuoDriveGUI.buttonY;
            for (int i = 0; i < NetworkController.hostData.Length; i++) {
                CreateClientButtons(buttonX, buttonY, NetworkController.hostData[i]);
                buttonY += buttonH * GameData.CARS_AMOUNT + 30;
            }
        }
    }
}
