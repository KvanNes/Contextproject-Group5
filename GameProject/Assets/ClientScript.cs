using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

public class ClientScript : MonoBehaviour {

	private float btnX;
	private float btnY;
	private float btnH;
	private float btnW;
	
	private bool refreshing;
	private HostData[] hostData;
	
	public string gameName = "CGCookie_DuoDrive";

	void Start() {
		Application.runInBackground = true;
		btnX = Screen.width * 0.05f;
		btnY = Screen.height * 0.05f;
		btnH = Screen.width * 0.1f;
		btnW = Screen.width * 0.1f;
	}

	void Update() {
		if(refreshing) {
			if(MasterServer.PollHostList().Length > 0) {
				refreshing = false;
				Debug.Log(MasterServer.PollHostList().Length);
				hostData = MasterServer.PollHostList();
			}
		}
	}

	private void startServer() {
		Network.InitializeServer(32, 25001, !Network.HavePublicAddress());
		MasterServer.RegisterHost(gameName, "Duo Drive", "Join this room!");
	}

	private void refreshHostList() {
		MasterServer.RequestHostList(gameName);
		refreshing = true;
	}

	void OnGUI() {
		if(!Network.isClient && !Network.isServer) {
			if(GUI.Button( new Rect(btnX, btnY, btnW, btnH), "Start Server")) {
				Debug.Log("Starting Server");
				startServer();
			}
			if(GUI.Button( new Rect(btnX, btnY * 1.2f + btnH, btnW, btnH), "Refresh Hosts")) {
				Debug.Log("Refresh Hosts");
				refreshHostList();
			}
			if(hostData != null) {
				for(int i = 0; i < hostData.Length; i++) {
					if(GUI.Button (new Rect(btnX * 1.5f + btnW, btnY * 1.2f + (btnH * i), btnW * 3f, btnH / 2f), hostData[i].gameName)) {
						Network.Connect(hostData[i]);
						Debug.Log(hostData[i]);
					}
				}
			}
		}
	}
}
