using Cars;
using Controllers;
using Interfaces;
using NetworkManager;
using UnityEngine;
using System;
using Utilities;

namespace GraphicalUI
{
    public class MainPart : GraphicalUIPart
    {

        private static readonly int buttonX = 10;
        private static readonly int buttonY = 75;
        private static readonly int buttonWidth = 150;
        private static readonly int buttonHeight = 50;

        private void CreateServerButton()
        {
            //if (!ServerAvailable())
            //{
            if (GUI.Button(new Rect(Screen.width / 2 - 75, Screen.height / 2 - 75, 150, 150), "Start server!"))
            {
                MainScript.SelfType = MainScript.PlayerType.Server;
                MainScript.Server.StartServer();
                NetworkController.Connected = true;
            }
            //}
        }

        private void CreateClientButton(Type type, HostData hostData, int carNumber, int x, int y)
        {
            string teamColorText = carNumber == 0 ? "Team Red\n" : "Team Blue\n";
            if (GUI.Button(new Rect(x, y, buttonWidth, buttonHeight), teamColorText + type.Name))
            {
                MainScript.SelfType = MainScript.PlayerType.Client;
                MainScript.SelfCar = new Car(carNumber);

                //Based on: http://stackoverflow.com/a/755
                MainScript.SelfPlayer = new Player(MainScript.SelfCar, (IPlayerRole)Activator.CreateInstance(type));
                MainScript.SelfPlayer.Role.Initialize();

                MainScript.Client.ChooseJobWhenConnected(type.Name, carNumber);
                Network.Connect(hostData);
                NetworkController.Connected = true;
            }
        }

        private void CreateClientButtons(int startX, int startY, HostData hostData)
        {
            for (int j = 0; j < GameData.CARS_AMOUNT; j++)
            {
                int y = startY + j * buttonHeight;
                Type[] roleTypes = { typeof(Driver), typeof(Throttler) };
                int roleCounter = 0;
                foreach (Type role in roleTypes)
                {
                    CreateClientButton(role, hostData, j, startX + roleCounter, y);
                    roleCounter += buttonWidth;
                }
            }
        }

        private void CreateTutorialButton()
        {
            if (GUI.Button(new Rect(10, 10, 100, 50), new GUIContent("Tutorial")))
            {
                MainScript.GUIController.Add(GraphicalUIController.TutorialConfiguration); //Tutorial.Show();
            }
        }

        private bool ServerAvailable()
        {
            return NetworkController.HostData != null && NetworkController.HostData.Length > 0;
        }

        public override void DrawGraphicalUI()
        {
            if (Application.internetReachability != NetworkReachability.ReachableViaLocalAreaNetwork)
            {
                return;
            }

            //Based on: http://answers.unity3d.com/questions/296204/gui-font-size.html
            GUI.skin.label.fontSize = GUI.skin.button.fontSize = 20;

            if (!Network.isServer)
            {
                CreateServerButton();
            }

            CreateTutorialButton();

            int buttonY_ = buttonY;
            if (ServerAvailable() && !Network.isServer && !Network.isClient)
            {
                for (int i = 0; i < NetworkController.HostData.Length; i++)
                {
                    CreateClientButtons(buttonX, buttonY_, NetworkController.HostData[i]);
                    buttonY_ += buttonHeight * GameData.CARS_AMOUNT + 30;
                }
            }
        }
    }
}
