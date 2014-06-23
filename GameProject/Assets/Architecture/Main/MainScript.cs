using System.Linq;
using Cars;
using Controllers;
using GraphicalUI;
using Interfaces;
using NetworkManager;
using UnityEngine;
using System.Collections.Generic;
using Utilities;
using Wrappers;

namespace Main
{
    public class MainScript : MonoBehaviour
    {

        public enum PlayerType
        {
            Server,
            Client,
            None
        };

        public static NetworkController NetworkController;
        public static Server Server;
        public static Client Client;
        public static List<Car> Cars;
        public static Player SelfPlayer;
        public static ICar SelfCar { get; set; }
        public static PlayerType SelfType = PlayerType.None;
        public static bool SelectionIsFinal = false;
        public static GraphicalUIController GUIController;
        public static CountdownController CountdownController;

        public static bool FixedCamera = false;
        public static int AmountPlayersConnected = 0;
        public static bool PlayerHasDisconnected = false;

        public void Start()
        {
            if (Application.platform == RuntimePlatform.WindowsEditor
                || Application.platform == RuntimePlatform.WindowsPlayer
                || Application.platform == RuntimePlatform.OSXPlayer
                || Application.platform == RuntimePlatform.OSXEditor)
            {
                Application.runInBackground = true;
            }
            else
            {
                Application.runInBackground = false;
            }

            InvokeRepeating("SendToOther", GameData.UPDATE_TIME_DELTA, GameData.UPDATE_TIME_DELTA);

            if (!GameData.USE_HARDCODED_IP)
            {
                InvokeRepeating("UpdateHostList", 0f, 5f);
            }

            Initialize();
        }

        public void Initialize()
        {
            Server = (Server)GameObject.FindGameObjectWithTag("Network").GetComponent(typeof(Server));
            Client = (Client)GameObject.FindGameObjectWithTag("Network").GetComponent(typeof(Client));

            // Use the NetworkWrapper so that the Server maintains it's actual functionallity.
            Server.Network = new NetworkWrapper();

            Cars = new List<Car>();
            for (int i = 0; i < GameData.CARS_AMOUNT; i++)
            {
                Cars.Add(new Car());
            }

            NetworkController = (NetworkController)GameObject.FindGameObjectWithTag("Network").GetComponent(typeof(NetworkController));
            GUIController = (GraphicalUIController)GameObject.FindGameObjectWithTag("GUI").GetComponent(typeof(GraphicalUIController));
            CountdownController = (CountdownController)GameObject.FindGameObjectWithTag("CountdownController").GetComponent(typeof(CountdownController));
        }

        public void UpdateHostList()
        {
            if (!NetworkController.Connected)
            {
                NetworkController.RefreshHostList();
            }
        }

        private bool CheckLANConnected()
        {
            if (Application.internetReachability != NetworkReachability.ReachableViaLocalAreaNetwork)
            {
                Network.Disconnect();
                Application.Quit();
                return false;
            }

            return true;
        }

        public void SendToOther()
        {
            if (!CheckLANConnected())
            {
                return;
            }

            if (SelfCar != null)
            {
                SelfCar.SendToOther();
            }
        }

        public static bool AllFinished()
        {
            return Cars.Where(car => car != null && car.CarObject != null).All(car => car.CarObject.Finished);
        }

        public static double TimeAtPlace(bool first)
        {
            List<double> finishingTimes = new List<double>(GameData.CARS_AMOUNT);
            foreach (Car car in Cars)
            {
                finishingTimes.Add(car.CarObject.FinishedTime);
            }
            finishingTimes.Sort();
            return first ? finishingTimes[0] : finishingTimes[1];
        }

        public Server GetServer()
        {
            return Server;
        }

        public Client GetClient()
        {
            return Client;
        }

        public List<Car> GetCars()
        {
            return Cars;
        }

        public void SetSelfCar(ICar c)
        {
            SelfCar = c;
        }

        public ICar GetSelfCar()
        {
            return SelfCar;
        }
    }
}