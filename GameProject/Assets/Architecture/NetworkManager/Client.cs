using Behaviours;
using Cars;
using Controllers;
using Interfaces;
using UnityEngine;
using Wrappers;

namespace NetworkManager
{
    public class Client : MonoBehaviour
    {
        public Player Player { get; set; }
        public Car Car { get; set; }

        private string _pendingType = "";
        private int _pendingCarNumber = -1;

        public INetwork Network { get; set; }
        public INetworkView NetworkView { get; set; }

        public void Start()
        {
            Network = new NetworkWrapper();
            NetworkView = new NetworkViewWrapper();
            NetworkView.SetNativeNetworkView(GetComponent<NetworkView>());
        }

        public void ChooseJobWhenConnected(string typeString, int carNumber)
        {
            _pendingType = typeString;
            _pendingCarNumber = carNumber;
        }

        public void OnConnectedToServer()
        {
            NetworkView.RPC("chooseJob", RPCMode.Server, _pendingType, _pendingCarNumber);
        }

        public void OnDisconnectedFromServer()
        {
            foreach (GameObject go in GameObject.FindGameObjectsWithTag("Player"))
            {
                DestroyImmediate(go);
            }
            NetworkController.Connected = false;
            MainScript.SelfCar = null;
            MainScript.SelfPlayer = null;
            MainScript.SelfType = MainScript.PlayerType.None;
            MainScript.SelectionIsFinal = false;
            Camera.main.transform.position = Vector3.zero;
            MainScript.GUIController.Remove();
        }

        private CarBehaviour GetCarObjectByNumber(int carNumber)
        {
            GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("Player");
            foreach (GameObject gObj in gameObjects)
            {
                CarBehaviour ab = (CarBehaviour)gObj.GetComponent(typeof(CarBehaviour));
                if (ab.CarNumber == carNumber)
                {
                    return ab;
                }
            }
            return null;
        }

        [RPC]
        public void chooseJobAvailable()
        {
            MainScript.SelfCar.CarObject = GetCarObjectByNumber(MainScript.SelfCar.CarNumber);
            MainScript.SelectionIsFinal = true;

            foreach (GameObject gObj in GameObject.FindGameObjectsWithTag("Player"))
            {
                Light[] lights = gObj.GetComponentsInChildren<Light>();
                if (lights.Length == 0)
                {
                    continue;
                }
                if (MainScript.SelfCar.CarObject.gameObject == gObj)
                {
                    lights[0].enabled = true;
                }
                else
                {
                    lights[0].enabled = false;
                }
            }
        }

        [RPC]
        public void chooseJobNotAvailable()
        {
            Network.Disconnect();
        }

        public string GetPendingType()
        {
            return _pendingType;
        }

        public int GetPendingCarNumber()
        {
            return _pendingCarNumber;
        }
    }
}