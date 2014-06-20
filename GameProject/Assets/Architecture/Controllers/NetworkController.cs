// Based on: http://cgcookie.com/unity/2011/12/20/introduction-to-networking-in-unity/

using Cars;
using Interfaces;
using Main;
using UnityEngine;
using System;
using Utilities;
using Wrappers;

namespace Controllers
{
    // This class takes care of sending and receiving data over a network.
    public class NetworkController : MonoBehaviour
    {

        public static HostData[] HostData;
        public static Boolean Connected = false;
        public INetworkView NetworkView;

        public void Start()
        {
            NetworkView = new NetworkViewWrapper();
            NetworkView.SetNativeNetworkView(GetComponent<NetworkView>());
        }

        public static bool ServerAvailable()
        {
            return GameData.USE_HARDCODED_IP || (HostData != null && HostData.Length > 0);
        }

        [RPC]
        public void RestartGame()
        {
            MainScript.CountdownController.StartCountdown();
            foreach (Car car in MainScript.Cars)
            {
                car.CarObject.Finished = false;
            }
            if (MainScript.SelfPlayer != null && MainScript.SelfPlayer.Role != null)
            {
                MainScript.SelfPlayer.Role.Restart();
            }
        }

        public static void RefreshHostList()
        {
            MasterServer.RequestHostList(GameData.GAME_NAME);
        }

        public void Update()
        {
            if (!GameData.USE_HARDCODED_IP
                && Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork)
            {
                if (MasterServer.PollHostList().Length > 0)
                {
                    HostData = MasterServer.PollHostList();
                }
            }
        }

        [RPC]
        public void ToggleLight()
        {
            if (MainScript.SelfPlayer.Role is Driver)
            {
                RenderSettings.ambientLight = RenderSettings.ambientLight == Color.white ? Color.black : Color.white;
            }
        }

        [RPC]
        public void ToggleOverview()
        {
            if (!(MainScript.SelfPlayer.Role is Throttler)) return;
            MainScript.FixedCamera = !MainScript.FixedCamera;
            Camera.main.transform.rotation = Quaternion.Euler(0, MainScript.FixedCamera ? 180 : 0, 0);
        }

        public void BroadcastAmountPlayers(int newAmount)
        {
            if (NetworkView.GetType() == typeof(NetworkViewWrapper) || NetworkView.GetType() == typeof(NetworkView))
            {
                NetworkView.RPC("UpdateAmountPlayers", RPCMode.All, newAmount);
            }
            if (newAmount == 4)
            {
                MainScript.Server.Game.ResetGame();
            }
        }

        [RPC]
        public void UpdateAmountPlayers(int amountPlayers)
        {
            MainScript.AmountPlayersConnected = amountPlayers;
        }

    }
}