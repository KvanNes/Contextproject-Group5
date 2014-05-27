using UnityEngine;
using System.Collections;

public class DuoDriveGUI : MonoBehaviour
{

    private HostData[] hostData = NetworkManager.hostData;

    private readonly static int buttonX = 10;
    private readonly static int buttonY = 10;
    private readonly static int buttonW = 150;
    private readonly static int buttonH = 50;

    public static bool connected = false;

    /**
     * Method that returns if there is even a server available.
     */
    public bool ServerAvailable() {
        return hostData != null && hostData.Length > 0;
    }

    public bool IsServer() {
        return Network.isServer;
    }

    /**
    * The method that handles all the GUI-events.
    */
    void OnGUI() {
        if (Application.internetReachability != NetworkReachability.ReachableViaLocalAreaNetwork) {
            return;
        }

        if (connected) {
            return;
        }

        // Refresh the hosts.
        if (!NetworkManager.refreshing) {
            NetworkManager.refreshHostList();
            hostData = NetworkManager.hostData;
        }
        
        // There still needs to be a server, create option to be a server.
        if (!IsServer() && !Network.isClient) {
            if (GUI.Button(new Rect(Screen.width / 2 - 75, Screen.height / 2 - 75, 150, 150), "Start server!")) {
                MainScript.selfType = MainScript.PlayerType.Server;
                NetworkManager.startServer();
            }
        }

        // There is a server available, let's be a client.
        if (ServerAvailable() && !Network.isClient) {
            int buttonY = DuoDriveGUI.buttonY;
            for (int i = 0; i < hostData.Length; i++) {
                // Using const here causes an error dialog to pop up frequently in MonoDevelop.
                string[] PLAYER_TYPES = { "steerer", "throttler" };
                int k = 0;
                foreach(string type in PLAYER_TYPES) {
                    for(int j = 0; j < GameData.CARS_AMOUNT; j++) {
                        int x = buttonX + k * buttonW;
                        int y = buttonY + j * buttonH;
                        if (GUI.Button(new Rect(x, y, buttonW, buttonH), hostData[i].gameName + " " + type + " " + j.ToString())) {
                            MainScript.selfType = MainScript.PlayerType.Client;
                            MainScript.selfCar = MainScript.cars[j];
                            MainScript.selfPlayer = (type == "throttler" ? (Player) new Throttler(MainScript.selfCar) : (Player) new Driver(MainScript.selfCar));
                            NetworkManager.chooseJobFromGUI(type, j);
                            NetworkManager.Connect(hostData[i]);
                            connected = true;
                        }
                    }
                    k++;
                }
                buttonY += buttonH * GameData.CARS_AMOUNT + 30;
            }
        }
    }
}
