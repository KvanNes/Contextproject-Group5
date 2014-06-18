using Behaviours;
using Cars;
using GraphicalUI;
using Interfaces;
using Moq;
using NUnit.Framework;
using UnityEngine;
using Utilities;

namespace GraphicalUITests
{
    public class RestartButtonTest
    {
        private RestartButtonPart _restartButtonPart;

        private GameObject _gameObject;
        private CarBehaviour _autoBehaviour;
        private Car _car;

        private Mock<INetworkView> _networkViewMock;

        [SetUp]
        public void SetUp()
        {
            _restartButtonPart = new RestartButtonPart();

            _gameObject = Object.Instantiate(Resources.LoadAssetAtPath("Assets/CarRed.prefab", typeof(GameObject))) as GameObject;
            _networkViewMock = new Mock<INetworkView>();

            if (_gameObject == null) return;
            _autoBehaviour = _gameObject.AddComponent<CarBehaviour>();
            _car = new Car(_autoBehaviour) { CarObject = { NetworkView = _networkViewMock.Object } };
        }

        [TearDown]
        public void Clear()
        {
            Utils.DestroyObject(_gameObject);
        }

        [Test]
        public void Test_ResetCar()
        {
            const float expected = 0f;

            _restartButtonPart.ResetCar(_car, default(Vector3));

            Assert.AreEqual(expected, _car.CarObject.Acceleration);

            _networkViewMock.Verify(net => net.RPC(It.IsAny<string>(), It.IsAny<RPCMode>(), It.IsAny<Vector3>(), It.IsAny<float>(), It.IsAny<int>()));
            _networkViewMock.Verify(net => net.RPC(It.IsAny<string>(), It.IsAny<RPCMode>(), It.IsAny<Quaternion>(), It.IsAny<int>()));
        }
    }
}