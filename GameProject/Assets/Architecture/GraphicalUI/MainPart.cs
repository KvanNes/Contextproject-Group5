using Cars;
using Controllers;
using Interfaces;
using Main;
using UnityEngine;
using System;
using Utilities;

namespace GraphicalUI
{
    public class MainPart : GraphicalUIPart
    {

        private static readonly int buttonY = 20 + 64 * 2 + 20;
        private static readonly float ButtonWidth = (float) Screen.width / 3;
        private static readonly float ButtonHeight = (float) Screen.height / 9;
        private Texture2D _textureStartServer;
        private Texture2D _textureDriverRed;
        private Texture2D _textureThrottlerRed;
        private Texture2D _textureDriverBlue;
        private Texture2D _textureThrottlerBlue;
        private Texture2D _textureTutorial;

        public override void Initialize()
        {
            _textureStartServer = TextureUtils.LoadTexture("button-start-server");
            _textureDriverRed = TextureUtils.LoadTexture("button-driver-red");
            _textureThrottlerRed = TextureUtils.LoadTexture("button-throttler-red");
            _textureDriverBlue = TextureUtils.LoadTexture("button-driver-blue");
            _textureThrottlerBlue = TextureUtils.LoadTexture("button-throttler-blue");
            _textureTutorial = TextureUtils.LoadTexture("button-tutorial");
        }

        private void CreateServerButton()
        {
            if (DrawTextureButton(new Rect(Screen.width / 2 - 1341 / 2, 20, 1341, 64), _textureStartServer))
            {
                MainScript.SelfType = MainScript.PlayerType.Server;
                MainScript.Server.StartServer();
                NetworkController.Connected = true;
            }
        }

        private void CreateClientButton(Type type, HostData hostData, int carNumber, float x, float y)
        {
            Texture2D texture;
            if (carNumber == 0)
            {
                texture = type == typeof(Driver) ? _textureDriverRed : _textureThrottlerRed;
            }
            else
            {
                texture = type == typeof(Driver) ? _textureDriverBlue : _textureThrottlerBlue;
            }

            if (DrawTextureButton(new Rect(x, y, ButtonWidth, ButtonHeight), texture))
            {
                MainScript.SelfType = MainScript.PlayerType.Client;
                MainScript.SelfCar = new Car(carNumber);

                //Based on: http://stackoverflow.com/a/755
                MainScript.SelfPlayer = new Player(MainScript.SelfCar, (IPlayerRole)Activator.CreateInstance(type));
                MainScript.SelfPlayer.Role.Initialize();

                MainScript.Client.ChooseJobWhenConnected(type.Name, carNumber);
                if (hostData == null)
                {
                    Network.Connect(GameData.IP, GameData.PORT);
                }
                else
                {
                    Network.Connect(hostData);
                }
                NetworkController.Connected = true;
            }
        }

        private void CreateClientButtons(float startY, HostData hostData)
        {
            for (int j = 0; j < GameData.CARS_AMOUNT; j++)
            {
                float y = startY + j * (ButtonHeight + 10);
                Type[] roleTypes = { typeof(Driver), typeof(Throttler) };
                foreach (Type role in roleTypes)
                {
                    CreateClientButton(role, hostData, j, role == typeof(Driver) ? 0 : Screen.width - ButtonWidth, y);
                }
            }
        }

        private void CreateClientButtonsForEachHost()
        {
            float buttonY_ = buttonY;
            if (NetworkController.ServerAvailable() && !Network.isServer && !Network.isClient)
            {
                if (GameData.USE_HARDCODED_IP)
                {
                    CreateClientButtons(buttonY_, null);
                }
                else
                {
                    for (int i = 0; i < NetworkController.HostData.Length; i++)
                    {
                        CreateClientButtons(buttonY_, NetworkController.HostData[i]);
                        buttonY_ += ButtonHeight * GameData.CARS_AMOUNT + 30;
                    }
                }
            }
        }

        private void CreateTutorialButton()
        {
            if (DrawTextureButton(new Rect(Screen.width / 2 - 1341 / 2, 20 + 64, 1341, 64), _textureTutorial))
            {
                MainScript.GuiController.Add(GraphicalUIController.TutorialConfiguration);
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

            // Based on: http://answers.unity3d.com/questions/296204/gui-font-size.html
            GUI.skin.label.fontSize = GUI.skin.button.fontSize = 20;

            if (!IsAndroid())
            {
                CreateServerButton();
            }

            CreateTutorialButton();

            CreateClientButtonsForEachHost();
        }
    }
}
