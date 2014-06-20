using System.Linq;
using Behaviours;
using Cars;
using Controllers;
using GraphicalUI;
using Interfaces;
using UnityEngine;
using Utilities;
using Wrappers;

namespace NetworkManager
{
    public class Server : MonoBehaviour
    {
        public Game Game { get; set; }
        public bool Connected = false;

        private GameObject _prefabGameObject;
        public GameObject FirstCarPrefab = null;
        public GameObject SecondCarPrefab = null;
        public Transform SpawnObject = null;

        public INetwork Network { get; set; }
        public INetworkView NetworkView { get; set; }

        public int AmountPlayersConnected = 0;

        public void StartServer()
        {
            NetworkView = new NetworkViewWrapper();
            NetworkView.SetNativeNetworkView(GetComponent<NetworkView>());
            Network.InitializeServer(32, GameData.PORT, false);//!UnityEngine.Network.HavePublicAddress());
            Connected = true;
            if (!Network.IsServer())
            {
                return;
            }
            if (!GameData.USE_HARDCODED_IP)
            {
                MasterServer.RegisterHost(GameData.GAME_NAME, "DuoDrive_Game");
            }
        }

        public void DisconnectServer()
        {
            if (IsConnected())
            {
                Network.Disconnect();
                Connected = false;
            }
        }

        public bool IsConnected()
        {
            return Connected;
        }

        [RPC]
        public bool checkJobAvailable(string typeString, int carNumber, NetworkPlayer networkPlayer)
        {
            if (carNumber < 0 || carNumber >= Game.Cars.Count)
            {
                return false;
            }

            Car car = Game.Cars[carNumber];
            if (car == null)
            {
                return false;
            }

            Player player = (typeString == "Throttler" ? car.Throttler : car.Driver);
            if (player.NetworkPlayer != default(NetworkPlayer))
            {
                return false;
            }

            if (typeString == "Throttler")
            {
                car.Throttler.NetworkPlayer = networkPlayer;
            }
            else
            {
                car.Driver.NetworkPlayer = networkPlayer;
            }
            return true;
        }

        [RPC]
        public void chooseJob(string typeString, int carNumber, NetworkMessageInfo info)
        {
            bool jobAvailable = checkJobAvailable(typeString, carNumber, info.sender);
            if (jobAvailable)
            {
                NetworkView.RPC("chooseJobAvailable", info.sender);

                AmountPlayersConnected += 1;
                MainScript.NetworkController.BroadcastAmountPlayers(AmountPlayersConnected);
            }
            else
            {
                NetworkView.RPC("chooseJobNotAvailable", info.sender);
            }
        }

        public static float GetStartingPosition(int carNumber)
        {
            return 0.45f - 0.3f * carNumber;
        }

        private void SpawnPlayer(int carNumber)
        {
            float y = GetStartingPosition(carNumber);
            Vector3 pos = SpawnObject.position + new Vector3(0, y, 0);
            _prefabGameObject = carNumber == 1 ? FirstCarPrefab : SecondCarPrefab;

            Object obj = Network.Instantiate(_prefabGameObject, pos, Quaternion.identity, 0);
            CarBehaviour ab = (CarBehaviour)((GameObject)obj).GetComponent(typeof(CarBehaviour));
            ab.NetworkView = new NetworkViewWrapper();
            ab.NetworkView.SetNativeNetworkView(ab.GetComponent<NetworkView>());
            Car car = new Car(ab);
            car.Throttler = new Player(car, new Throttler());
            car.Driver = new Player(car, new Driver());
            Game.AddCar(car);
            ab.NetworkView.RPC("setCarNumber", RPCMode.OthersBuffered, carNumber - 1);
        }

        private void OnServerInitialized()
        {
            Game = new Game();
            Camera.main.transform.position = new Vector3(0, 0, 10);
            for (int i = 1; i <= GameData.CARS_AMOUNT; i++)
            {
                SpawnPlayer(i);
            }
            MainScript.GUIController.Add(GraphicalUIController.ServerConfiguration);
        }

        public void OnPlayerDisconnected(NetworkPlayer player)
        {
            AmountPlayersConnected -= 1;
            MainScript.NetworkController.BroadcastAmountPlayers(AmountPlayersConnected);

            Network.RemoveRPCs(player);
            Network.DestroyPlayerObjects(player);

            foreach (Car car in Game.Cars)
            {
                if (car.Driver.NetworkPlayer == player)
                {
                    car.Driver.NetworkPlayer = default(NetworkPlayer);
                    break;
                }

                if (car.Throttler.NetworkPlayer == player)
                {
                    car.Throttler.NetworkPlayer = default(NetworkPlayer);
                    break;
                }
            }
        }
    }
}