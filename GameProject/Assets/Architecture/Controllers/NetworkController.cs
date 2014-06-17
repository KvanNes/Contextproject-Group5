// Gebaseerd op: http://cgcookie.com/unity/2011/12/20/introduction-to-networking-in-unity/

using Cars;
using Interfaces;
using NetworkManager;
using UnityEngine;
using System;
using Utilities;
using Wrappers;

namespace Controllers
{
    // This class takes care of sending and receiving data.
    public class NetworkController : MonoBehaviour
    {

        public static HostData[] hostData;
        public static Boolean connected = false;
        public INetworkView NetworkView;

        public void Start()
        {
            NetworkView = new NetworkViewWrapper();
            NetworkView.SetNativeNetworkView(GetComponent<NetworkView>());
        }

		[RPC]
		public void RestartGame() {
            MainScript.CountdownController.StartCountdown();
            TimeController.GetInstance().Reset();
     	}

        public static void RefreshHostList()
        {
            MasterServer.RequestHostList(GameData.GAME_NAME);
        }

        public void Update()
        {
            if (Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork)
            {
                if (MasterServer.PollHostList().Length > 0)
                {
                    hostData = MasterServer.PollHostList();
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

    }
}