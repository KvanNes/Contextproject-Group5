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

		private static int buttonX;// + Screen.width / 10 * 4 ;
		private static int buttonY;
		private static float buttonW;
		private static float buttonH;

		private static float XRedDriver;
		private static float YRedDriver;
		private static float XRedThrottler;
		private static float YRedThrottler;
		private static float XBlueDriver;
		private static float YBlueDriver;
		private static float XBlueThrottler;
		private static float YBlueThrottler;
		private float[] coordinates = new float[8] {XRedDriver,YRedDriver,XRedThrottler,YRedThrottler,XBlueDriver,YBlueDriver,XBlueThrottler,YBlueThrottler};
		
        
        private void CreateServerButton() {
			if (!ServerAvailable()) {
				GUI.color = Color.yellow;
				GUI.backgroundColor = Color.green;
				if (GUI.Button (new Rect (0, Screen.height / 2 - Screen.height / 3, Screen.width, Screen.height/11), "", GUIStyle.none)) {
								MainScript.SelfType = MainScript.PlayerType.Server;
								GameObject.FindGameObjectWithTag("BackgroundCamera").camera.enabled = false;
								MainScript.Server.StartServer ();
								NetworkController.connected = true;
						}
				}
        }
        
        private void CreateClientButton(Type type, int carNumber, float x, float y) {
			string teamColorText = carNumber == 0 ? "Team Red\n" : "Team Blue\n";
			//Screen.width / 2 + Screen.width / 3.7f , Screen.height /2 voor Team Blue driver
			//Screen.width / 2 - Screen.width / 2.3f , Screen.height /2 voor Team Blue Throttler
			//Screen.width / 2 - Screen.width / 2.325f , Screen.height /2 + Screen.height/7.4f voor team Red throttler
			//Screen.width / 2 + Screen.width / 3.7f , Screen.height /2 + Screen.height/7.4f voor team red driver
			if (GUI.Button(new Rect(x , y, buttonW, buttonH), "",GUIStyle.none)) {
                MainScript.SelfType = MainScript.PlayerType.Client;
                MainScript.SelfCar = new Car(carNumber);
                
                // Gebaseerd op: http://stackoverflow.com/a/755
                MainScript.SelfPlayer = new Player(MainScript.SelfCar, (IPlayerRole) Activator.CreateInstance(type));
                MainScript.SelfPlayer.Role.Initialize();                
				GameObject.FindGameObjectWithTag("BackgroundCamera").camera.enabled = false;
				GameObject.FindGameObjectWithTag("SecondBackGroundCamera").camera.enabled = false;
				MainScript.Client.ChooseJobWhenConnected(type.Name, carNumber);
				Network.Connect("127.0.0.1",GameData.PORT);
                NetworkController.connected = true;
            }
        }
        
        private void CreateClientButtons(float startX, float startY) {
		   int i = 0;
			InitiateCoordinates ();
           for(int j = 0; j < GameData.CARS_AMOUNT; j++) {
				//int y = startY + j * buttonH;
                IEnumerable<Type> roleTypes = Utils.TypesImplementingInterface(typeof(IPlayerRole));
                int roleCounter = 0;
                foreach(Type role in roleTypes) {
					float x = coordinates[i];
					i++;
					float y = coordinates[i];
					CreateClientButton(role, j, x, y);
					i++;
					//roleCounter += buttonW;
                }
            }
        }

		private void InitiateCoordinates(){
			buttonX = Screen.width / 2 - Screen.width / 4;
			buttonY = Screen.height / 2 + Screen.height /4;
			buttonH = Screen.height / 12;
			buttonW = Screen.width / 5.3f;
			coordinates[0] = Screen.width / 2 + Screen.width / 3.75f;
			coordinates[1] = Screen.height / 2 + Screen.height / 7.4f;
			coordinates [2] = Screen.width / 2 - Screen.width / 2.3f;
			coordinates [3] = Screen.height / 2 + Screen.height / 7.4f;
			coordinates [4] = Screen.width / 2 + Screen.width / 3.7f;
			coordinates [5] = Screen.height / 2;
			coordinates [6] = Screen.width / 2 -Screen.width / 2.3f;
			coordinates [7] = Screen.height / 2;
		}
        
        private void CreateTutorialButton() {
			if (GUI.Button(new Rect(0, Screen.height/2 - Screen.height/6, Screen.width, Screen.height/11),"",GUIStyle.none)) {
				GameObject.FindGameObjectWithTag("BackgroundCamera").camera.enabled = false;
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
            //if (ServerAvailable() && !Network.isServer && !Network.isClient) {
                for (int i = 0; i < 2 /*NetworkController.hostData.Length*/; i++) {
                    CreateClientButtons(buttonX, buttonY_/*, NetworkController.hostData[i]*/);
                    //buttonY_ += buttonH * GameData.CARS_AMOUNT;
                //}
			}
        }
    }
}
