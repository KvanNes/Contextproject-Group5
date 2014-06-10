using Cars;
using Controllers;
using Interfaces;
using NetworkManager;
using UnityEngine;
using System;
using System.Collections.Generic;
using Utilities;

public class DuoDriveGUI : MonoBehaviour {

    private static int buttonX = 10;
    private static int buttonY = 75;
    private static int buttonW = 150;
    private static int buttonH = 50;

    private bool ServerAvailable() {
        return NetworkController.hostData != null && NetworkController.hostData.Length > 0;
    }

    public void Start() {
        InvokeRepeating("UpdateHostList", 0f, 5f);
    }

    public void UpdateHostList() {
        if (!NetworkController.connected) {
            NetworkController.refreshHostList();
        }
    }

    private void CreateServerButton() {
        if (GUI.Button(new Rect(Screen.width / 2 - 75, Screen.height / 2 - 75, 150, 150), "Start server!")) {
            MainScript.SelfType = MainScript.PlayerType.Server;
            MainScript.Server.StartServer();
            NetworkController.connected = true;
        }
    }

    private void CreateClientButton(Type type, HostData hostData, int carNumber, int x, int y) {
        if (GUI.Button(new Rect(x, y, buttonW, buttonH), type.Name + "; Car " + carNumber)) {
            MainScript.SelfType = MainScript.PlayerType.Client;
            MainScript.SelfCar = new Car(carNumber);

            // Gebaseerd op: http://stackoverflow.com/a/755
            MainScript.SelfPlayer = new Player(MainScript.SelfCar, (IPlayerRole) Activator.CreateInstance(type));
            MainScript.SelfPlayer.Role.Initialize();                

            MainScript.Client.ChooseJobWhenConnected(type.Name, carNumber);
            Network.Connect(hostData);
            NetworkController.connected = true;
        }
    }

    private void CreateClientButtons(int startX, int startY, HostData hostData) {
        for(int j = 0; j < GameData.CARS_AMOUNT; j++) {
            int y = startY + j * buttonH;
            IEnumerable<Type> roleTypes = Utils.TypesImplementingInterface(typeof(IPlayerRole));
            int roleCounter = 0;
            foreach(Type role in roleTypes) {
                CreateClientButton(role, hostData, j, startX + roleCounter, y);
                roleCounter += buttonW;
            }
        }
    }

    private void CreateTutorialButton() {
        if (GUI.Button(new Rect(10, 10, 100, 50), new GUIContent("Tutorial"))) {
            MainScript.Tutorial.Show();
        }
    }

    public void OnGUI() {
        if (MainScript.Tutorial.Enabled || NetworkController.connected || Application.internetReachability != NetworkReachability.ReachableViaLocalAreaNetwork) {
            return;
        }

        // Gebaseerd op: http://answers.unity3d.com/questions/296204/gui-font-size.html
        GUI.skin.label.fontSize = GUI.skin.button.fontSize = 20;

        if (!Network.isServer) {
            CreateServerButton();
        }

        CreateTutorialButton();

        int buttonY_ = buttonY;
        if (ServerAvailable() && !Network.isServer && !Network.isClient) {
            for (int i = 0; i < NetworkController.hostData.Length; i++) {
                CreateClientButtons(buttonX, buttonY, NetworkController.hostData[i]);
                buttonY_ += buttonH * GameData.CARS_AMOUNT + 30;
            }
            buttonY = buttonY_;
        }
    }
}
