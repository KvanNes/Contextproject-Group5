using System.Collections.Generic;
using Behaviours;
using Cars;
using Controllers;
using Interfaces;
using Main;
using Moq;
using NetworkManager;
using NUnit.Framework;
using UnityEngine;
using Utilities;
using Wrappers;

namespace ControllersTests
{
    public class NetworkControllerTest
    {

        private GameObject _gameObject;
        private CarBehaviour _carBehaviour;
        private Server _server;

        private NetworkController _networkController;
        public Mock<INetworkView> NetworkView;

        [SetUp]
        public void SetUp()
        {
            _gameObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
            _networkController = _gameObject.AddComponent<NetworkController>();
            _carBehaviour = _gameObject.AddComponent<CarBehaviour>();
            _server = _gameObject.AddComponent<Server>();
            NetworkView = new Mock<INetworkView>();
            _networkController.NetworkView = NetworkView.Object;
            _carBehaviour.NetworkView = NetworkView.Object;

            MainScript.NetworkController = _networkController;
        }

        [TearDown]
        public void Clear()
        {
            Utils.DestroyObject(_gameObject);
        }

        [Test]
        public void Test_Start()
        {
            _networkController.Start();

            Assert.IsNotNull(_networkController.NetworkView);
            Assert.AreEqual(typeof(NetworkViewWrapper), _networkController.NetworkView.GetType());
        }

        [Test]
        public void Test_ToggleLight_White()
        {
            // Initialize for if-statement
            MainScript.SelfPlayer = new Player { Role = new Driver() };
            RenderSettings.ambientLight = Color.white;

            // Trigger the method
            _networkController.ToggleLight();

            Assert.AreEqual(Color.black, RenderSettings.ambientLight);
        }

        [Test]
        public void Test_ToggleLight_Other()
        {
            // Initialize for if-statement
            MainScript.SelfPlayer = new Player { Role = new Driver() };
            RenderSettings.ambientLight = Color.gray;

            // Trigger the method
            _networkController.ToggleLight();

            // Assertion
            Assert.AreEqual(Color.white, RenderSettings.ambientLight);
        }

        [Test]
        public void Test_ToggleOverview_FixedCamera()
        {
            // Expected value
            Quaternion expectedQuaternion = Quaternion.Euler(0, 180, 0);

            // Initialize first if-statement
            MainScript.SelfPlayer = new Player { Role = new Throttler() };

            // Initialize second if-statement
            MainScript.FixedCamera = false;

            // Trigger the method
            _networkController.ToggleOverview();

            // Assertion
            Assert.AreEqual(expectedQuaternion, Camera.main.transform.rotation);
        }

        [Test]
        public void Test_ToggleOverview_NotFixedCamera()
        {
            // Expected value
            Quaternion expectedQuaternion = Quaternion.Euler(0, 0, 0);

            // Initialize first if-statement
            MainScript.SelfPlayer = new Player { Role = new Throttler() };

            // Initialize second if-statement
            MainScript.FixedCamera = true;

            // Trigger the method
            _networkController.ToggleOverview();

            // Assertion
            Assert.AreEqual(expectedQuaternion, Camera.main.transform.rotation);
        }

        [Test]
        public void Test_BroadcastAmountPlayers()
        {
            const int newAmountPlayers = 4;

            MainScript.Server = _server;
            MainScript.Server.Game = new Game();
            Car car1 = new Car { CarObject = _carBehaviour };
            Car car2 = new Car { CarObject = _carBehaviour };
            List<Car> carsList = new List<Car> { car1, car2 };
            MainScript.Cars = carsList;

            GameObject gameObject = GameObject.CreatePrimitive(PrimitiveType.Cube); //GameObject.Find("SpawnPositionBase").GetComponent("Transform");
            gameObject.name = "SpawnPositionBase";
            gameObject.AddComponent<Transform>();

            _networkController.BroadcastAmountPlayers(newAmountPlayers, false);

            for (int i = 0; i < 4; i++)
                NetworkView.Verify(net => net.RPC(It.IsAny<string>(), It.IsAny<RPCMode>()));

            Utils.DestroyObject(gameObject);
        }
    }
}
