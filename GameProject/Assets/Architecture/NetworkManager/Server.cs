using Behaviours;
using Cars;
using Mock;
using UnityEngine;
using Utilities;

namespace NetworkManager
{
    public class Server : MonoBehaviour
    {
        public Game Game { get; set; }
        public bool Connected = false;

        public GameObject PlayerPrefab = null;
        public Transform SpawnObject = null;

        public INetwork Network { get; set; }
        public INetworkView NetworkView { get; set; }

        public void StartServer()
        {
            NetworkView = new NetworkViewWrapper();
            NetworkView.SetNativeNetworkView(GetComponent<NetworkView>());
            Network.InitializeServer(32, GameData.PORT, !UnityEngine.Network.HavePublicAddress());
            MasterServer.RegisterHost(GameData.GAME_NAME, "2P1C");
            Connected = true;
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
        public bool checkJobAvailableAndMaybeAdd(string typeString, int carNumber, NetworkPlayer networkPlayer)
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
            Debug.Log(UnityEngine.Network.player.port + " | " + default(NetworkPlayer).port);
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
            bool ok = checkJobAvailableAndMaybeAdd(typeString, carNumber, info.sender);
            if (ok)
            {
                NetworkView.RPC("chooseJobAvailable", info.sender);
            }
            else
            {
                NetworkView.RPC("chooseJobNotAvailable", info.sender);
            }
        }

        public static float GetStartingPosition(int carNumber)
        {
            return 0.12f - 0.05f * carNumber;
        }

        private void SpawnPlayer(int carNumber)
        {
            float y = GetStartingPosition(carNumber);
            Vector3 pos = SpawnObject.position + new Vector3(0, y, 0);

            Object obj = Network.Instantiate(PlayerPrefab, pos, Quaternion.identity, 0);
            AutoBehaviour ab = (AutoBehaviour)((GameObject)obj).GetComponent(typeof(AutoBehaviour));
            Car car = new Car(ab);
            car.Throttler = new Player(car, new Throttler());
            car.Driver = new Player(car, new Driver());
            Game.AddCar(car);
            ab.networkView.RPC("setCarNumber", RPCMode.OthersBuffered, carNumber - 1);
        }

        private void OnServerInitialized()
        {
            Game = new Game();
            Camera.main.transform.position = new Vector3(0, 0, 10);
            for (int i = 1; i <= GameData.CARS_AMOUNT; i++)
            {
                SpawnPlayer(i);
            }
        }

        private void OnGUI()
        {
            if (Game != null)
            {
                // Gebaseerd op: http://answers.unity3d.com/questions/296204/gui-font-size.html
                GUI.skin.label.fontSize = 20;

                GUI.Label(new Rect(10, 10, 200, 50), new GUIContent("Server started"));
            }
        }

        private void OnPlayerConnected(NetworkPlayer player)
        {

        }

        private void OnPlayerDisconnected(NetworkPlayer player)
        {
            UnityEngine.Network.RemoveRPCs(player);
            UnityEngine.Network.DestroyPlayerObjects(player);

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