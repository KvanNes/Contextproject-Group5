using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MainScript : MonoBehaviour {

    public enum PlayerType {
        Server,
        Client,
        None
    };
    
    public static NetworkController networkController;
    public static Server server;
    public static Client client;
    public static List<Car> Cars;
    public static Player SelfPlayer;
    public static ICar SelfCar { get; set; }
    public static PlayerType selfType = PlayerType.None;
    public static bool selectionIsFinal = false;

    // FIXME: Remove the following variables in release.
    public static bool isDebug = false;
    public static bool fixedCamera = false;

	// Use this for initialization
    void Start () {
        if (Application.platform == RuntimePlatform.WindowsEditor
            || Application.platform == RuntimePlatform.WindowsPlayer
            || Application.platform == RuntimePlatform.OSXPlayer
            || Application.platform == RuntimePlatform.OSXEditor) {
            Application.runInBackground = true;
        } else {
            Application.runInBackground = false;
        }

        InvokeRepeating("SendToOther", GameData.UPDATE_TIME_DELTA, GameData.UPDATE_TIME_DELTA);

        Initialize();
	}

    public void Initialize()
    {
        server = (Server)GameObject.FindGameObjectWithTag("Network").GetComponent(typeof(Server));
        client = (Client)GameObject.FindGameObjectWithTag("Network").GetComponent(typeof(Client));

        // Use the NetworkWrapper so that the Server maintains it's actual functionallity.
        server.network = new NetworkWrapper();

        Cars = new List<Car>();
        for (int i = 0; i < GameData.CARS_AMOUNT; i++)
        {
            Cars.Add(new Car());
        }
    }

    public void SendToOther() {
        if (Application.internetReachability != NetworkReachability.ReachableViaLocalAreaNetwork) {
            // Make sure to only send/receive data on local network.
            Network.Disconnect();
            Application.Quit();
            return;
        }

        if (SelfCar != null) {
            SelfCar.SendToOther();
        }
    }

    public Server GetServer()
    {
        return server;
    }

    public Client GetClient()
    {
        return client;
    }

    public List<Car> GetCars()
    {
        return Cars;
    }

    public void SetSelfCar(ICar c)
    {
        SelfCar = c;
    }

    public void Clear()
    {
        server = null;
        client = null;
        Cars.Clear();
    }
}
