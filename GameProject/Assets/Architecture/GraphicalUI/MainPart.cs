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
        private static readonly int buttonY = 20 + 64 * 2 + 20;
        private static readonly int buttonWidth = 172;
        private static readonly int buttonHeight = 43;
        private Texture2D TextureStartServer;
        private Texture2D TextureDriverRed;
        private Texture2D TextureThrottlerRed;
        private Texture2D TextureDriverBlue;
        private Texture2D TextureThrottlerBlue;
        private Texture2D TextureTutorial;

        public override void Initialize() {
            TextureStartServer = TextureUtils.LoadTexture("button-start-server");
            TextureDriverRed = TextureUtils.LoadTexture("button-driver-red");
            TextureThrottlerRed = TextureUtils.LoadTexture("button-throttler-red");
            TextureDriverBlue = TextureUtils.LoadTexture("button-driver-blue");
            TextureThrottlerBlue = TextureUtils.LoadTexture("button-throttler-blue");
            TextureTutorial = TextureUtils.LoadTexture("button-tutorial");
        }

        private void CreateServerButton()
        {
            if (DrawTextureButton(new Rect(Screen.width / 2 - 1341 / 2, 20, 1341, 64), TextureStartServer))
            {
                MainScript.SelfType = MainScript.PlayerType.Server;
                MainScript.Server.StartServer();
                NetworkController.Connected = true;
            }
        }

        private void CreateClientButton(Type type, HostData hostData, int carNumber, int x, int y)
        {
            Texture2D texture;
            if (carNumber == 0) {
                texture = type == typeof(Driver) ? TextureDriverRed : TextureThrottlerRed;
            } else {
                texture = type == typeof(Driver) ? TextureDriverBlue : TextureThrottlerBlue;
            }

            if (DrawTextureButton(new Rect(x, y, buttonWidth, buttonHeight), texture))
            {
                MainScript.SelfType = MainScript.PlayerType.Client;
                MainScript.SelfCar = new Car(carNumber);

                //Based on: http://stackoverflow.com/a/755
                MainScript.SelfPlayer = new Player(MainScript.SelfCar, (IPlayerRole)Activator.CreateInstance(type));
                MainScript.SelfPlayer.Role.Initialize();

                MainScript.Client.ChooseJobWhenConnected(type.Name, carNumber);
                if(hostData == null) {
                    Network.Connect(GameData.IP, GameData.PORT);
                } else {
                    Network.Connect(hostData);
                }
                NetworkController.Connected = true;
            }
        }

        private void CreateClientButtons(int startX, int startY, HostData hostData)
        {
            for (int j = 0; j < GameData.CARS_AMOUNT; j++)
            {
                int y = startY + j * (buttonHeight + 10);
                Type[] roleTypes = { typeof(Driver), typeof(Throttler) };
                foreach (Type role in roleTypes)
                {
                    CreateClientButton(role, hostData, j, role == typeof(Driver) ? 0 : Screen.width - buttonWidth, y);
                }
            }
        }

        private void CreateTutorialButton()
        {
            if (DrawTextureButton(new Rect(Screen.width / 2 - 1341 / 2, 20 + 64, 1341, 64), TextureTutorial))
            {
                MainScript.GUIController.Add(GraphicalUIController.TutorialConfiguration);
            }
        }

        private bool IsAndroid()
        {
            return Application.platform == RuntimePlatform.Android;
        }

        public override void BecomeVisible()
        {
            Camera.main.backgroundColor = Color.white;
        }

        public override void DrawGraphicalUI()
        {
            if (Application.internetReachability != NetworkReachability.ReachableViaLocalAreaNetwork)
            {
                return;
            }

            //Based on: http://answers.unity3d.com/questions/296204/gui-font-size.html
            GUI.skin.label.fontSize = GUI.skin.button.fontSize = 20;

            if (!IsAndroid())
            {
                CreateServerButton();
            }

            CreateTutorialButton();

            int buttonY_ = buttonY;
            if (NetworkController.ServerAvailable() && !Network.isServer && !Network.isClient)
            {
                if(GameData.USE_HARDCODED_IP) {
                    CreateClientButtons(buttonX, buttonY_, null);
                } else {
                    for (int i = 0; i < NetworkController.HostData.Length; i++)
                    {
                        CreateClientButtons(buttonX, buttonY_, NetworkController.HostData[i]);
                        buttonY_ += buttonHeight * GameData.CARS_AMOUNT + 30;
                    }
                }
            }
        }
    }
}
