// Gebaseerd op: http://cgcookie.com/unity/2011/12/20/introduction-to-networking-in-unity/

using Cars;
using NetworkManager;
using UnityEngine;
using System;
using Utilities;
using UnityException = Utilities.UnityException;

namespace Controllers
{
    // This class takes care of sending and receiving data.
    public class NetworkController : MonoBehaviour
    {

        public static HostData[] hostData;
		public static Boolean connected = false;

		public int countDownValue = 0;
		public bool allowedToDrive = false;

        public void Start()
        {
            if (MainScript.NetworkController != null)
            {
                throw new UnityException("NetworkController is being created twice, it seems");
            }

            MainScript.NetworkController = this;
        }

		[RPC]
		public void StartCountdown() {
			allowedToDrive = false;
			countDownValue = 3;
			InvokeRepeating("DecrementCounter", 0f, 1f);
		}
		
		private void DecrementCounter() {
			if(--countDownValue == 0) {
				allowedToDrive = true;
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