// Gebaseerd op: http://cgcookie.com/unity/2011/12/20/introduction-to-networking-in-unity/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using AssemblyCSharp;

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

}
