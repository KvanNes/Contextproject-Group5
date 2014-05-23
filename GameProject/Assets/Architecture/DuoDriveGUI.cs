using UnityEngine;
using System.Collections;

public class DuoDriveGUI : MonoBehaviour
{

    private HostData[] hostData = NetworkManager.hostData;
    private bool logoShown = false;

    private int buttonX = 10;
    private int buttonY = 10;
    private int buttonW = 150;
    private int buttonH = 50;

    /**
     * Method that returns if there is even a server available.
     */
    public bool ServerAvailable()
    {
        return hostData != null && hostData.Length > 0;
    }

    /**
     * Show the logo (currently a label) before the game really starts.
     */
    IEnumerator Wait(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        logoShown = true;
    }
    bool connected = false;

    enum playerType {
        steerer,
        throttler
    }

    /**
    * The method that handles all the GUI-events.
    */
    void OnGUI()
    {
        if (connected)
        {
            return;
        }
        // Refresh the hosts.
        if (!NetworkManager.refreshing)
        {
            NetworkManager.refreshHostList();
            hostData = NetworkManager.hostData;
        }

        // We need the logo to be shown for 3 seconds.
        if (!logoShown)
        {
            GUI.Label(new Rect(Screen.width / 2 - 75, Screen.height / 2 - 50, 150, 100), "DuoDrive");
            StartCoroutine(Wait(0.0f));
        }
        else
        {
            // There is a server available, let's be a client.
            if (ServerAvailable() && !Network.isClient)
            {
                if (hostData.Length == -1)  // FIXME
                {
                    StartCoroutine(Wait(1.0f));
                    NetworkManager.Connect(hostData[0]);
                    connected = true;
                }
                else
                {
                    buttonY = 0;
                    const int CARS = 2;
                    for (int i = 0; i < hostData.Length; i++)
                    {
                        // Using const here causes error dialogs to pop up frequently on MonoDevelop.
                        playerType[] PLAYER_TYPES = { playerType.steerer, playerType.throttler };
                        int k = 0;
                        foreach(playerType pt in PLAYER_TYPES) {
                            for(int j = 0; j < CARS; j++) {
                                int x = buttonX + k * buttonW;
                                int y = buttonY + j * buttonH;
                                string type = (pt == playerType.steerer ? "steerer" : "throttler");
                                if (GUI.Button(new Rect(x, y, buttonW, buttonH), hostData[i].gameName + " " + type + " " + j.ToString()))
                                {
                                    NetworkManager.chooseJobFromGUI(type, j);
                                    NetworkManager.Connect(hostData[i]);
                                    connected = true;
                                }
                            }
                            k++;
                        }
                        buttonY += buttonH * CARS + 30;
                    }
                }
            }

            // There still needs to be a server, create option to be a server.
            if (/*!ServerAvailable() && */!Network.isServer && !Network.isClient)
            {
                if (GUI.Button(new Rect(Screen.width / 2 - 75, Screen.height / 2 - 75, 150, 150), "Start server!"))
                {
                    Debug.Log("Starting the server");
                    NetworkManager.startServer();
                }
            }
        }
    }
}
