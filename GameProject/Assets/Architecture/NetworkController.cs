// Gebaseerd op: http://cgcookie.com/unity/2011/12/20/introduction-to-networking-in-unity/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

// This class takes care of sending and receiving data.

public class NetworkController : MonoBehaviour {

    public static HostData[] hostData;
    public static Boolean connected = false;

    private void Start() {
        if (MainScript.networkController != null) {
            throw new UnityException("NetworkController is being created twice, it seems");
        }

        MainScript.networkController = this;
    }

    public static void refreshHostList() {
        MasterServer.RequestHostList(GameData.GAME_NAME);
    }

    public void Update() {
        if (Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork) {
            if (MasterServer.PollHostList().Length > 0) {
                hostData = MasterServer.PollHostList();
            }
        }
    }

    [RPC]
    public void ToggleLight() {
        if (MainScript.selfPlayer.Role is Driver) {
            RenderSettings.ambientLight = RenderSettings.ambientLight == Color.white ? Color.black : Color.white;
        }
    }

    [RPC]
    public void ToggleOverview() {
        if(MainScript.selfPlayer.Role is Throttler) {
            MainScript.fixedCamera = !MainScript.fixedCamera;
            if(MainScript.fixedCamera) {
                Camera.main.transform.rotation = Quaternion.Euler(0, 180, 0);
            } else {
                Camera.main.transform.rotation = Quaternion.Euler(0, 0, 0);
            }
        }
    }

}
