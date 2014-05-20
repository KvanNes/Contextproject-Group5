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
            StartCoroutine(Wait(3.0f));
        }
        else
        {
            // There is a server available, let's be a client.
            if (ServerAvailable() && !Network.isClient)
            {
                if (hostData.Length == 1)
                {
                    StartCoroutine(Wait(1.0f));
                    //Network.Connect(hostData[0]);
                    NetworkManager.Connect(hostData[0]);
                    connected = true;
                }
                else
                {
                    for (int i = 0; i < hostData.Length; i++)
                    {
                        if (GUI.Button(new Rect(buttonX, buttonY, buttonW, buttonH), hostData[i].gameName))
                        {
                            //Network.Connect(hostData[i]);
                            //Debug.Log(hostData[i]);
                            NetworkManager.Connect(hostData[i]);
                            connected = true;
                        }
                        buttonY += 30;
                    }
                }
            }

            // There still needs to be a server, create option to be a server.
            if (!ServerAvailable() && !Network.isServer)
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
