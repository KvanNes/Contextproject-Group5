using Cars;
using Controllers;
using Interfaces;
using NetworkManager;
using UnityEngine;
using System;
using System.Collections.Generic;
using Utilities;

namespace GraphicalUI {
    public class MainPart : GraphicalUIPart {

		private static int buttonX = Screen.width / 2 - Screen.width / 3;// + Screen.width / 10 * 4 ;
		private static int buttonY = Screen.height / 2 + Screen.height /3;
        private static int buttonW = Screen.width / 3; //150;
		private static int buttonH = Screen.height / 8; //50;

		private static float XRedDriver = Screen.width / 2 + Screen.width / 1.1f;
		private static float YRedDriver = Screen.height /2 + Screen.height/1.5f;
		private static float XRedThrottler = Screen.width / 2 - Screen.width / 2.7f;
		private static float YRedThrottler = Screen.height / 2 + Screen.height / 1.5f;
		private static float XBlueDriver = Screen.width / 2 + Screen.width / 1.1f;
		private static float YBlueDriver = Screen.height / 2 + Screen.height / 2.4f;
		private static float XBlueThrottler = Screen.width / 2 - Screen.width / 2.7f;
		private static float YBlueThrottler = Screen.height / 2 + Screen.height / 2.4f;
		private float[] coordinates = new float[8] {XRedDriver,YRedDriver,XRedThrottler,YRedThrottler,XBlueDriver,YBlueDriver,XBlueThrottler,YBlueThrottler};
		
        
        private void CreateServerButton() {
			if (!ServerAvailable()) {
				GUI.color = Color.yellow;
				GUI.backgroundColor = Color.green;
				if (GUI.Button (new Rect (0, Screen.height / 2 - Screen.height / 3, Screen.width, Screen.height/11), "", GUIStyle.none)) {
								MainScript.SelfType = MainScript.PlayerType.Server;
								MainScript.Server.StartServer ();
								NetworkController.connected = true;
						}
				}
        }
        
        private void CreateClientButton(Type type, HostData hostData, int carNumber, float x, float y) {
			string teamColorText = carNumber == 0 ? "Team Red\n" : "Team Blue\n";
			//Screen.width / 2 + Screen.width / 3.7f , Screen.height /2 voor Team Blue driver
			//Screen.width / 2 - Screen.width / 2.3f , Screen.height /2 voor Team Blue Throttler
			//Screen.width / 2 - Screen.width / 2.325f , Screen.height /2 + Screen.height/7.4f voor team Red throttler
			//Screen.width / 2 + Screen.width / 3.7f , Screen.height /2 + Screen.height/7.4f voor team red driver
			if (GUI.Button(new Rect(x , y, buttonW, buttonH), teamColorText + type.Name)) {
                MainScript.SelfType = MainScript.PlayerType.Client;
                MainScript.SelfCar = new Car(carNumber);
                
                // Gebaseerd op: http://stackoverflow.com/a/755
                MainScript.SelfPlayer = new Player(MainScript.SelfCar, (IPlayerRole) Activator.CreateInstance(type));
                MainScript.SelfPlayer.Role.Initialize();                
				GameObject.FindGameObjectWithTag("BackgroundCamera").camera.enabled = false;
				MainScript.Client.ChooseJobWhenConnected(type.Name, carNumber);
                Network.Connect(hostData);
                NetworkController.connected = true;
            }
        }
        
        private void CreateClientButtons(int startX, int startY, HostData hostData) {
		   int i = 0;
           for(int j = 0; j < GameData.CARS_AMOUNT; j++) {
				//int y = startY + j * buttonH;
                IEnumerable<Type> roleTypes = Utils.TypesImplementingInterface(typeof(IPlayerRole));
                int roleCounter = 0;
                foreach(Type role in roleTypes) {
					float x = coordinates[i];
					i++;
					float y = coordinates[i];
					CreateClientButton(role, hostData, j, x, y);
					i++;
					roleCounter += buttonW;
                }
            }
        }
        
        private void CreateTutorialButton() {
			if (GUI.Button(new Rect(0, Screen.height/2 - Screen.height/6, Screen.width, Screen.height/11), new GUIContent("Tutorial"), GUIStyle.none)) {
                MainScript.GUIController.Add(GraphicalUIController.TutorialConfiguration); //Tutorial.Show();
            }
        }
        
        private bool ServerAvailable() {
            return NetworkController.hostData != null && NetworkController.hostData.Length > 0;
        }

        public override void DrawGraphicalUI() {
            if (Application.internetReachability != NetworkReachability.ReachableViaLocalAreaNetwork) {
                return;
            }
            
            // Gebaseerd op: http://answers.unity3d.com/questions/296204/gui-font-size.html
            //GUI.skin.label.fontSize = GUI.skin.button.fontSize = 20;
            
            if (!Network.isServer) {
                CreateServerButton();
            }
            
            CreateTutorialButton();
            
            int buttonY_ = buttonY;
            if (ServerAvailable() && !Network.isServer && !Network.isClient) {
                for (int i = 0; i < NetworkController.hostData.Length; i++) {
                    CreateClientButtons(buttonX, buttonY_, NetworkController.hostData[i]);
                    buttonY_ += buttonH * GameData.CARS_AMOUNT;
                }
            }
        }
    }
}
