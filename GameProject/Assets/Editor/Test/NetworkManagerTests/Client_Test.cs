using Controllers;
using Mock;
using Moq;
using NetworkManager;
using NUnit.Framework;
using UnityEngine;
using Utilities;

namespace MainTests
{
    [TestFixture]
    public class ClientTest
    {
        private Client _client;
        private GameObject _gameObject;

        public Mock<INetwork> Network;
        public Mock<INetworkView> NetworkView;

        [SetUp]
        public void SetUp()
        {
            _gameObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
            _client = _gameObject.AddComponent<Client>();
            Network = new Mock<INetwork>();
            NetworkView = new Mock<INetworkView>();
            _client.NetworkView = NetworkView.Object;
            _client.Network = Network.Object;
        }

        [TearDown]
        public void Clear()
        {
            Utils.DestroyObject(_gameObject);
        }

        [Test]
        public void Test_Start()
        {
            _client.Start();

            Assert.IsTrue(_client.Network.GetType() == typeof(NetworkWrapper));
            Assert.IsTrue(_client.NetworkView.GetType() == typeof(NetworkViewWrapper));
        }

        [Test]
        public void Test_ChooseJobWhenConnected()
        {
            const string typeString = "A";
            const int carNumber = 0;

            _client.ChooseJobWhenConnected(typeString, carNumber);

            Assert.AreEqual(typeString, _client.GetPendingType());
            Assert.AreEqual(carNumber, _client.GetPendingCarNumber());
        }

        [Test]
        public void Test_OnConnectedToServer()
        {
            _client.OnConnectedToServer();

            NetworkView.Verify(net => net.RPC(It.IsAny<string>(), It.IsAny<RPCMode>(), It.IsAny<string>(), It.IsAny<int>()));
        }

        [Test]
        public void Test_OnDisconnectedFromServer()
        {
            _client.OnDisconnectedFromServer();

            Assert.IsEmpty(GameObject.FindGameObjectsWithTag("Player"));
            Assert.IsFalse(NetworkController.connected);
            Assert.IsNull(MainScript.SelfCar);
            Assert.IsNull(MainScript.SelfPlayer);
            Assert.IsFalse(MainScript.SelectionIsFinal);
            Assert.AreEqual(MainScript.PlayerType.None, MainScript.SelfType);
            Assert.AreEqual(Vector3.zero, Camera.main.transform.position);
        }

        [Test]
        public void Test_ChooseJobNotAvailable()
        {
            _client.chooseJobNotAvailable();

            Network.Verify(net => net.Disconnect());
        }

        // TODO: Write the test Test_ChooseJobAvailable()
    }
}
