using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

public class ServerScript : MonoBehaviour {

	public GameObject playerPrefab = null;
	public Transform spawnObject = null;

	void spawnPlayer(float y) {
		Vector3 pos = spawnObject.position + new Vector3(1f, y, 0);
		Network.Instantiate(playerPrefab, pos, Quaternion.identity, 0);
	}
	
	[RPC]
	void setAvailablePosition(int position) {
		if (position != -1) {
			spawnPlayer(0.07f - 0.05f * position);
		} else {
			GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("Player");
			foreach(GameObject obj in gameObjects) {
				Destroy(obj);
			}
			Network.Disconnect();
		}
	}
	
	void OnServerInitialized() {
		setAvailablePosition(0);
	}
	
	List<bool> beschikbaar = new List<bool> { false, true }; //, true, true };
	Dictionary<NetworkPlayer, int> beschikbaarWie = new Dictionary<NetworkPlayer, int>();
	
	void OnPlayerConnected(NetworkPlayer player) {
		networkView.RPC("setAvailablePosition", player, availablePosition(player));
	}
	
	void OnPlayerDisconnected(NetworkPlayer player) {
		Network.RemoveRPCs(player);
		Network.DestroyPlayerObjects(player);
		int value;
		if (beschikbaarWie.TryGetValue(player, out value)) {
			beschikbaar[value] = true;
			beschikbaarWie.Remove(player);
		}
	}
	
	int availablePosition(NetworkPlayer networkPlayer) {
		for (int i = 0; i < beschikbaar.Count; i++) {
			if (beschikbaar[i]) {
				beschikbaar[i] = false;
				beschikbaarWie.Add(networkPlayer, i);
				return i;
			}
		}
		return -1;
	}
}
