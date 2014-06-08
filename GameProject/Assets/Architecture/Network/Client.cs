using Behaviours;
using Cars;
using Controllers;
using UnityEngine;

namespace NetworkManager
{
    public class Client : MonoBehaviour
    {
        public Player Player { get; set; }
        public Car Car { get; set; }

        private string _pendingType = "";
        private int _pendingCarNumber = -1;

        public void ChooseJobWhenConnected(string typeString, int carNumber)
        {
            _pendingType = typeString;
            _pendingCarNumber = carNumber;
        }

        public void OnConnectedToServer()
        {
            networkView.RPC("chooseJob", RPCMode.Server, _pendingType, _pendingCarNumber);
        }

        public void OnDisconnectedFromServer()
        {
            foreach (GameObject go in GameObject.FindGameObjectsWithTag("Player"))
            {
                Destroy(go);
            }
            NetworkController.connected = false;
            MainScript.SelfCar = null;
            MainScript.SelfPlayer = null;
            MainScript.SelfType = MainScript.PlayerType.None;
            MainScript.SelectionIsFinal = false;
            Camera.main.transform.position = Vector3.zero;
        }

        private AutoBehaviour GetCarObjectByNumber(int carNumber)
        {
            GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("Player");
            foreach (GameObject gObj in gameObjects)
            {
                AutoBehaviour ab = (AutoBehaviour)gObj.GetComponent(typeof(AutoBehaviour));
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
    }
}