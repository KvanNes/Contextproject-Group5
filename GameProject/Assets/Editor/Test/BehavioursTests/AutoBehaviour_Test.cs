using Behaviours;
using Cars;
using Interfaces;
using NetworkManager;
using UnityEngine;
using NUnit.Framework;
using Moq;
using System.Collections.Generic;
using Utilities;
using Object = UnityEngine.Object;

namespace BehavioursTests
{
    [TestFixture]
    public class AutoBehaviourTest
    {
        // Mock Objects
        public Mock<INetworkView> NetworkView;

        // Objects
        private CarBehaviour _autoBehaviour;
        private GameObject _gameObject;

        /*
         * Setup for the tests by creating the appropiate mocks and setting up the server
         */
        [SetUp]
        public void Setup()
        {
            NetworkView = new Mock<INetworkView>();

            _gameObject =
                Object.Instantiate(Resources.LoadAssetAtPath("Assets/CarRed.prefab", typeof(GameObject))) as GameObject;
            if (_gameObject != null)
            {
                _autoBehaviour = _gameObject.AddComponent<CarBehaviour>();
                _gameObject.GetComponent<CarBehaviour>().NetworkView = NetworkView.Object;
            }

            var cars = new List<Car>();
            for (var i = 0; i < GameData.CARS_AMOUNT; i++)
            {
                _autoBehaviour.NetworkView = NetworkView.Object;
                var c = new Car(_autoBehaviour);
                cars.Add(c);
            }
            MainScript.Cars = cars;
            MainScript.SelfCar = new Car { CarObject = _autoBehaviour };
            MainScript.SelfPlayer = new Player { Role = new Driver() };
        }

        [TearDown]
        public void Clear()
        {
            Utils.DestroyObject(_gameObject);
        }

        [Test]
        public void Test_setCarNumber()
        {
            const int initialCarNumber = 0;

            _autoBehaviour.setCarNumber(initialCarNumber);
            Assert.AreEqual(initialCarNumber, _autoBehaviour.CarNumber);
            Assert.AreEqual(_autoBehaviour, MainScript.Cars[initialCarNumber].CarObject);
        }

        [Test]
        public void Test_requestInitialPositions()
        {
            _autoBehaviour.requestInitialPositions(new NetworkMessageInfo());

            for (int i = 0; i < MainScript.Cars.Count; i++)
            {
                NetworkView.Verify(
                    net =>
                        net.RPC(It.IsAny<string>(), It.IsAny<NetworkPlayer>(), It.IsAny<Vector3>(), It.IsAny<float>(),
                            It.IsAny<int>()));
                NetworkView.Verify(
                    net =>
                        net.RPC(It.IsAny<string>(), It.IsAny<NetworkPlayer>(), It.IsAny<Quaternion>(), It.IsAny<int>()));
            }
        }

        [Test]
        public void Test_UpdateRotation()
        {
            _autoBehaviour.CarNumber = 10;

            var rot = _autoBehaviour.transform.rotation;
            rot.Set(2.0F, 2.0F, 2.0F, 2.0F);
            var rotEuler = rot.eulerAngles;

            _autoBehaviour.UpdateRotation(rot, 10);

            Assert.AreEqual(rotEuler, _autoBehaviour.transform.rotation.eulerAngles);
        }

    }
}