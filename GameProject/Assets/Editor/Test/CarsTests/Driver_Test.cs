using Behaviours;
using Cars;
using Mock;
using NetworkManager;
using UnityEngine;
using NUnit.Framework;
using Moq;
using Utilities;
using Object = UnityEngine.Object;

namespace CarsTests
{
    [TestFixture]
    public class DriverTest
    {
        // Mock Objects
        public Mock<INetworkView> NetworkView;
        public Mock<INetworkView> NetworkViewOther;

        // Objects
        private Player _player;
        private Driver _driver;

        private GameObject _gameObject;
        private AutoBehaviour _autoBehaviour;

        private GameObject _gameObjectOther;
        private AutoBehaviour _autoBehaviourOther;

        private Car _carDriver;
        private Car _carOther;

        private float _midScreenWidth;

        [SetUp]
        public void SetUp()
        {
            _midScreenWidth = (float)Screen.width / 2;

            _driver = new Driver();
            NetworkView = new Mock<INetworkView>();
            NetworkViewOther = new Mock<INetworkView>();

            _gameObject =
                Object.Instantiate(Resources.LoadAssetAtPath("Assets/Auto.prefab", typeof(GameObject))) as GameObject;
            if (_gameObject == null) return;
            _autoBehaviour = _gameObject.AddComponent<AutoBehaviour>();
            _autoBehaviour.NetworkView = NetworkView.Object;

            _gameObjectOther =
                Object.Instantiate(Resources.LoadAssetAtPath("Assets/Auto.prefab", typeof(GameObject))) as GameObject;
            if (_gameObjectOther == null) return;
            _autoBehaviourOther = _gameObjectOther.AddComponent<AutoBehaviour>();
            _autoBehaviourOther.NetworkView = NetworkViewOther.Object;

            _carDriver = new Car(_autoBehaviour);
            _player = new Player(_carDriver, _driver);
            _carOther = new Car(_autoBehaviourOther);

            MainScript.SelfPlayer = new Player { Role = new Driver() };
        }

        [TearDown]
        public void Clear()
        {
            Utils.DestroyObject(_gameObject);
            Utils.DestroyObject(_gameObjectOther);
            InputWrapper.Clear();
        }

        [Test]
        public void Test_SendToOther()
        {
            Quaternion rot = _autoBehaviour.transform.rotation;

            _carOther.CarObject.transform.rotation = rot;
            _player.Role.SendToOther(_carOther);

            Assert.AreEqual(Utils.Copy(rot), _driver.GetLastSentRotation());
            NetworkViewOther.Verify(
                net => net.RPC(It.IsAny<string>(), It.IsAny<RPCMode>(), It.IsAny<Quaternion>(), It.IsAny<int>()));
        }

        [Test]
        public void Test_GetPlayerAction_None()
        {
            PlayerAction action = _driver.GetPlayerAction();
            Assert.AreEqual(PlayerAction.None, action);
        }

        [Test]
        public void Test_GetPlayerAction_LeftArrow()
        {
            InputWrapper.SetKey(KeyCode.LeftArrow, true);
            PlayerAction paNew = _driver.GetPlayerAction();

            Assert.AreEqual(PlayerAction.SteerLeft, paNew);
        }

        [Test]
        public void Test_GetPlayerAction_RightArrow()
        {
            InputWrapper.SetKey(KeyCode.RightArrow, true);
            PlayerAction paNew = _driver.GetPlayerAction();

            Assert.AreEqual(PlayerAction.SteerRight, paNew);
        }

        [Test]
        public void Test_GetPlayerAction_TouchLeft()
        {
            PlayerAction paPrev = _driver.GetPlayerAction();
            InputWrapper.SetTouchCount(1);
            InputWrapper.SetTouch(0, new Vector2(_midScreenWidth - 10.0f, 1.0f));
            PlayerAction paNew = _driver.GetPlayerAction();

            Assert.AreNotEqual(paPrev, paNew);
            Assert.AreEqual(PlayerAction.SteerLeft, paNew);
        }

        [Test]
        public void Test_GetPlayerAction_TouchRight()
        {
            PlayerAction paPrev = _driver.GetPlayerAction();
            InputWrapper.SetTouchCount(1);
            InputWrapper.SetTouch(0, new Vector2(_midScreenWidth + 10.0f, 1.0f));
            PlayerAction paNew = _driver.GetPlayerAction();

            Assert.AreNotEqual(paPrev, paNew);
            Assert.AreEqual(PlayerAction.SteerRight, paNew);
        }

        [Test]
        public void Test_HandlePlayerAction_Left()
        {
            InputWrapper.SetKey(KeyCode.LeftArrow, true);

            _autoBehaviour.Speed = 1.0f;
            Vector3 oldRotation = _autoBehaviour.transform.rotation.eulerAngles;

            _driver.HandlePlayerAction(_autoBehaviour);

            Assert.AreNotEqual(oldRotation, _autoBehaviour.transform.rotation.eulerAngles);
        }

        [Test]
        public void Test_HandlePlayerAction_Right()
        {
            InputWrapper.SetKey(KeyCode.RightArrow, true);

            _autoBehaviour.Speed = 1.0f;
            Vector3 oldRotation = _autoBehaviour.transform.rotation.eulerAngles;

            _driver.HandlePlayerAction(_autoBehaviour);

            Assert.AreNotEqual(oldRotation, _autoBehaviour.transform.rotation.eulerAngles);
        }
    }
}