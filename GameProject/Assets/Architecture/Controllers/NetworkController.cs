// Gebaseerd op: http://cgcookie.com/unity/2011/12/20/introduction-to-networking-in-unity/

using Cars;
using NetworkManager;
using UnityEngine;
using System;
using Utilities;
using UnityException = UnityEngine.UnityException;

namespace Controllers
{
    // This class takes care of sending and receiving data.
    public class NetworkController : MonoBehaviour
    {

        public static HostData[] hostData;
        public static Boolean connected = false;

        private void Start()
        {
            if (MainScript.NetworkController != null)
            {
                throw new UnityException("NetworkController is being created twice, it seems");
            }

            MainScript.NetworkController = this;
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
            if (MainScript.SelfPlayer.Role is Throttler)
            {
                MainScript.FixedCamera = !MainScript.FixedCamera;
                if (MainScript.FixedCamera)
                {
                    Camera.main.transform.rotation = Quaternion.Euler(0, 180, 0);
                }
                else
                {
                    Camera.main.transform.rotation = Quaternion.Euler(0, 0, 0);
                }
            }
        }

    }
}