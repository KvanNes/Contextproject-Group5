﻿using Cars;
using Controllers;
using Mock;
using UnityEngine;
using System.Collections.Generic;
using Utilities;

namespace NetworkManager
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
        public static Tutorial Tutorial;

        // FIXME: Remove the following variables in release.
        public static bool IsDebug = false;
        public static bool FixedCamera = false;

        // Use this for initialization
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

            Tutorial = (Tutorial)GameObject.FindGameObjectWithTag("Tutorial").GetComponent(typeof(Tutorial));
        }

        public void SendToOther()
        {
            if (Application.internetReachability != NetworkReachability.ReachableViaLocalAreaNetwork)
            {
                // Make sure to only send/receive data on local network.
                Network.Disconnect();
                Application.Quit();
                return;
            }

            if (SelfCar != null)
            {
                SelfCar.SendToOther();
            }
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

        public void Clear()
        {
            Server = null;
            Client = null;
            Cars.Clear();
        }
    }
}
