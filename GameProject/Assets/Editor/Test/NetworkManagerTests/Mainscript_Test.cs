using Cars;
using Mock;
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

            _mainScript = _gameObject.AddComponent<MainScript>();
            _selfCar = new Mock<ICar>();
        }

        [TearDown]
        public void Cleanup()
        {
            Utils.DestroyObject(_gameObject);
            Utils.DestroyObject(_gObjNetwork);
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

        [Test]
        public void Test_Clear()
        {
            _mainScript.Initialize();

            _mainScript.Clear();

            Assert.IsNull(_mainScript.GetServer());
            Assert.IsNull(_mainScript.GetClient());
            Assert.IsEmpty(_mainScript.GetCars());
        }

    }
}