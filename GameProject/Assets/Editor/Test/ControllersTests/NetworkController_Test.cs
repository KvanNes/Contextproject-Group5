using Controllers;
using NetworkManager;
using NUnit.Framework;
using UnityEngine;
using Utilities;

namespace ControllersTests
{
    public class NetworkControllerTest
    {

        private GameObject _gameObject;

        private NetworkController _networkController;

        [SetUp]
        public void SetUp()
        {
            _gameObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
            _networkController = _gameObject.AddComponent<NetworkController>();
        }

        [TearDown]
        public void Clear()
        {

        }

        [Test]
        public void Test_Start_Null()
        {
            MainScript.NetworkController = null;

            _networkController.Start();

            Assert.AreEqual(_networkController, MainScript.NetworkController);
        }

        [Test]
        [ExpectedException(typeof(Utilities.UnityException))]
        public void Test_Start_NotNull()
        {
            MainScript.NetworkController = _networkController;

            _networkController.Start();
        }
    }
}
