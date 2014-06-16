// Gebaseerd op: http://cgcookie.com/unity/2011/12/20/introduction-to-networking-in-unity/

using Cars;
using Interfaces;
using NetworkManager;
using UnityEngine;
using System;
using Utilities;
using Wrappers;
using UnityException = Utilities.UnityException;

namespace Controllers
{
    // This class takes care of sending and receiving data.
    public class NetworkController : MonoBehaviour
    {

        public static HostData[] hostData;
        public static Boolean connected = false;
        public INetworkView NetworkView;
		public int countDownValue = -100;  // FIXME: This value should cause the countdown not be shown when starting the app.
		public bool allowedToDrive = false;

        public void Start()
        {
            if (MainScript.NetworkController != null)
            {
                throw new UnityException("NetworkController is being created twice, it seems");
            }

            MainScript.NetworkController = this;
            MainScript.NetworkController.NetworkView = new NetworkViewWrapper();
            MainScript.NetworkController.NetworkView.SetNativeNetworkView(GetComponent<NetworkView>());

        }

		[RPC]
		public void StartCountdown() {
			allowedToDrive = false;
			countDownValue = 3;
            CancelInvoke("DecrementCounter");
			InvokeRepeating("DecrementCounter", 1f, 1f);
		}
		
        private void DecrementCounter() {
            countDownValue--;
			if (countDownValue == 0) {
                allowedToDrive = true;
            }
            if (countDownValue == -4) {
				CancelInvoke("DecrementCounter");
            }
		}

        public static void refreshHostList()
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