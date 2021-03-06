﻿using Cars;
using Controllers;
using GraphicalUI;
using Interfaces;
using Main;
using NetworkManager;
using UnityEngine;
using NUnit.Framework;
using Moq;
using Utilities;

namespace MainTests
{
    [TestFixture]
    public class MainscriptTest
    {
        private MainScript _mainScript;
        private GameObject _gameObject;
        private GameObject _gObjNetwork;
        private GameObject _gObjTutorial;
        private GameObject _gObjGui;
        private GameObject _gObjCountDownController;

        private Mock<ICar> _selfCar;

        [SetUp]
        public void SetUp()
        {
            _gameObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
            _gObjNetwork = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            _gObjNetwork.AddComponent<Server>();
            _gObjNetwork.GetComponent<Server>().tag = "Network";
            _gObjNetwork.AddComponent<Client>();
            _gObjNetwork.GetComponent<Client>().tag = "Network";
            _gObjNetwork.tag = "Network";

            _gObjGui = GameObject.CreatePrimitive(PrimitiveType.Cube);
            _gObjGui.AddComponent<GraphicalUIController>();
            _gObjGui.tag = "GUI";

            _gObjCountDownController = GameObject.CreatePrimitive(PrimitiveType.Cube);
            _gObjCountDownController.AddComponent<CountdownController>();
            _gObjCountDownController.tag = "CountdownController";

            _mainScript = _gameObject.AddComponent<MainScript>();
            _selfCar = new Mock<ICar>();
        }

        [TearDown]
        public void Cleanup()
        {
            Utils.DestroyObject(_gameObject);
            Utils.DestroyObject(_gObjNetwork);
            Utils.DestroyObject(_gObjGui);
            Utils.DestroyObject(_gObjCountDownController);
        }

        // TESTS FOR THE METHOD INITIALIZE
        [Test]
        public void Test_ServerSet()
        {
            _mainScript.Initialize();
            Assert.IsNotNull(_mainScript.GetServer());
        }

        [Test]
        public void Test_ClientSet()
        {
            _mainScript.Initialize();
            Assert.IsNotNull(_mainScript.GetClient());
        }

        [Test]
        public void Test_ServerNetwork()
        {
            _mainScript.Initialize();
            Assert.IsNotNull(_mainScript.GetServer().Network);
        }

        [Test]
        public void Test_Cars()
        {
            _mainScript.Initialize();
            Assert.AreEqual(GameData.CARS_AMOUNT, _mainScript.GetCars().Count);
        }

        [Test]
        public void Test_CarObjects()
        {
            _mainScript.Initialize();
            foreach (Car car in _mainScript.GetCars())
            {
                Assert.IsNotNull(car);
            }
        }

        [Test]
        public void Test_SendToOther()
        {
            _mainScript.SetSelfCar(_selfCar.Object);

            _mainScript.SendToOther();
            _selfCar.Verify(car => car.SendToOther());
        }

        [Test]
        public void Test_SetSelfCar()
        {
            _mainScript.SetSelfCar(_selfCar.Object);
            Assert.IsNotNull(_mainScript.GetSelfCar());
        }
    }
}