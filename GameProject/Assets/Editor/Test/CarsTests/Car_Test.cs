using Behaviours;
using Cars;
using Interfaces;
using Main;
using UnityEngine;
using NUnit.Framework;
using Utilities;
using Moq;

namespace CarsTests
{
    [TestFixture]
    public class CarTest
    {
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

            _carCarNumber = new Car(0);

            _driverRole = new Driver();
            _throttlerRole = new Throttler();

            _driver = new Player();
            _throttler = new Player();

            _driver.Role = _driverRole;
            _throttler.Role = _throttlerRole;
        }

        [TearDown]
        public void Clear()
        {
            Utils.DestroyObject(_gameObject);
            _car.Reset();
            _carAutoBehaviour.Reset();
            _carCarNumber.Reset();
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
            Assert.AreEqual(0, _carCarNumber.CarNumber);
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
            Assert.AreEqual(1, _car.CarNumber);
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
            MainScript.SelfCar = _carAutoBehaviour;
			_carAutoBehaviour.CarObject.PositionInitialized = true;
			_carAutoBehaviour.CarObject.RotationInitialized = true;

            PlayerRoleMock = new Mock<IPlayerRole>();
            MainScript.SelfPlayer = new Player { Role = PlayerRoleMock.Object };

            _carAutoBehaviour.SendToOther();

            PlayerRoleMock.Verify(role => role.SendToOther(It.IsAny<Car>()));
        }
    }
}