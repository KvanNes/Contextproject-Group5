using Behaviours;
using Cars;
using GraphicalUI;
using Interfaces;
using Moq;
using NUnit.Framework;
using UnityEngine;

namespace GraphicalUITests
{
    public class RestartButtonTest
    {
        private RestartButtonPart _restartButtonPart;

        private GameObject _gameObject;
        private AutoBehaviour _autoBehaviour;
        private Car _car;

        private Mock<INetworkView> _networkViewMock;

        [SetUp]
        public void SetUp()
        {
            _restartButtonPart = new RestartButtonPart();

            _gameObject = Object.Instantiate(Resources.LoadAssetAtPath("Assets/CarRed.prefab", typeof(GameObject))) as GameObject;
            _networkViewMock = new Mock<INetworkView>();

            if (_gameObject == null) return;
            _autoBehaviour = _gameObject.AddComponent<AutoBehaviour>();
            _car = new Car(_autoBehaviour) { CarObject = { NetworkView = _networkViewMock.Object } };
        }

        [Test]
        public void Test_ResetCar()
        {
            const float expected = 0f;
            Vector3 expectedVector3 = new Vector3(25f / 0.3f, 0f, -0.3f);
            Quaternion expectedQuaternion = Quaternion.identity;

            _restartButtonPart.ResetCar(_car, default(Vector3));

            Assert.AreEqual(expected, _car.CarObject.Speed);
            Assert.AreEqual(expected, _car.CarObject.Acceleration);

            Assert.AreEqual(expectedQuaternion, _car.CarObject.transform.rotation);
            Assert.AreEqual(expectedVector3, _car.CarObject.GetSphere().transform.localPosition);
            Assert.AreEqual(expectedQuaternion, _car.CarObject.GetSphere().transform.localRotation);

            _networkViewMock.Verify(net => net.RPC(It.IsAny<string>(), It.IsAny<RPCMode>(), It.IsAny<Vector3>(), It.IsAny<float>(), It.IsAny<int>()));
            _networkViewMock.Verify(net => net.RPC(It.IsAny<string>(), It.IsAny<RPCMode>(), It.IsAny<Quaternion>(), It.IsAny<int>()));
            _networkViewMock.Verify(net => net.RPC(It.IsAny<string>(), It.IsAny<RPCMode>()));
        }

        [Test]
        public void Test_ResetTimer()
        {
            RestartButtonPart.ResetTimer();

            Assert.AreEqual(Time.time, RestartButtonPart.TimerStart);
        }
    }
}