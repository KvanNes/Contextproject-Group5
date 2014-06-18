using Behaviours;
using Cars;
using Interfaces;
using NetworkManager;
using UnityEngine;
using NUnit.Framework;
using Utilities;
using Moq;

namespace CarsTests
{
    [TestFixture]
    public class CarTest
    {
        private const int Zero = 0;
        private const int One = 1;

        private GameObject _gameObject;

        private Car _car;
        private Car _carCarNumber;
        private Car _carAutoBehaviour;
        private CarBehaviour _autoBehaviour;

        private Player _driver;
        private Player _throttler;

        private IPlayerRole _driverRole;
        private IPlayerRole _throttlerRole;

        public Mock<IPlayerRole> PlayerRoleMock;

        [SetUp]
        public void SetUp()
        {
            _car = new Car();
            _gameObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
            _autoBehaviour = _gameObject.AddComponent<CarBehaviour>();

            _carAutoBehaviour = new Car(_autoBehaviour);

            _carCarNumber = new Car(Zero);

            _driverRole = new Driver();
            _throttlerRole = new Throttler();

            _driver = new Player();
            _throttler = new Player();

            _driver.Role = _driverRole;
            _throttler.Role = _throttlerRole;
        }

		//              CONSTRUCTORS:
        // Constructor with carNumber
        [Test]
        public void Test_ConstructorCarNumber()
        {
            Assert.IsNotNull(_carCarNumber);
        }

        [Test]
        public void Test_CarNumberSet()
        {
            Assert.AreEqual(Zero, _carCarNumber.CarNumber);
        }

        // Constructor plain
        [Test]
        public void Test_ConstructorPlain()
        {
            Assert.IsNotNull(_car);
        }

        [Test]
        public void Test_CarNumberUp()
        {
            Assert.AreEqual(One, _car.CarNumber);
        }

        // Constructor with AutoBehaviour
        [Test]
        public void Test_ConstructorAutoBehaviour()
        {
            Assert.IsNotNull(_carAutoBehaviour);
        }

        [Test]
        public void Test_AutoBehaviourSet()
        {
            Assert.AreEqual(_autoBehaviour, _carAutoBehaviour.CarObject);
        }

        [Test]
        public void Test_SendToOther()
        {
            const int initialized = 3; 

            MainScript.SelfCar = _carAutoBehaviour;
            _carAutoBehaviour.CarObject.Initialized = initialized;

            PlayerRoleMock = new Mock<IPlayerRole>();
            MainScript.SelfPlayer = new Player {Role = PlayerRoleMock.Object};

            _carAutoBehaviour.SendToOther();

            PlayerRoleMock.Verify(role => role.SendToOther(It.IsAny<Car>()));
        }
    }
}