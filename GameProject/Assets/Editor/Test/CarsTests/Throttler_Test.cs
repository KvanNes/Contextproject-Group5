using Behaviours;
using Cars;
using Controllers;
using Interfaces;
using Main;
using UnityEngine;
using NUnit.Framework;
using Moq;
using Utilities;
using Wrappers;

namespace CarsTests
{
    [TestFixture]
    public class ThrottlerTest
    {

        // Mock Objects
        public Mock<INetworkView> NetworkView;
        public Mock<INetworkView> NetworkViewOther;

        // Objects
        private Player _player;
        private Throttler _throttler;

        private GameObject _gameObject;
        private CarBehaviour _autoBehaviour;
        private Car _carDriver;

        private GameObject _gameObjectOther;
        private CarBehaviour _autoBehaviourOther;
        private Car _carOther;

        private GameObject _gameObjectCountDownController;
        private CountdownController _countdownController;

        private float _midScreenWidth;
        private const float Delta = 0.001f;

        [SetUp]
        public void SetUp()
        {
            _midScreenWidth = (float)Screen.width / 2;

            _throttler = new Throttler();
            NetworkView = new Mock<INetworkView>();
            NetworkViewOther = new Mock<INetworkView>();

            _gameObject =
                Object.Instantiate(Resources.LoadAssetAtPath("Assets/CarRed.prefab", typeof(GameObject))) as GameObject;
            if (_gameObject == null) return;
            _autoBehaviour = _gameObject.AddComponent<CarBehaviour>();
            _gameObject.GetComponent<CarBehaviour>().NetworkView = NetworkView.Object;
            _carDriver = new Car(_autoBehaviour) { CarObject = { NetworkView = NetworkView.Object } };
            _player = new Player(_carDriver, _throttler);

            _gameObjectOther =
                Object.Instantiate(Resources.LoadAssetAtPath("Assets/CarBlue.prefab", typeof(GameObject))) as GameObject;
            if (_gameObjectOther == null) return;
            _autoBehaviourOther = _gameObjectOther.AddComponent<CarBehaviour>();
            _gameObjectOther.GetComponent<CarBehaviour>().NetworkView = NetworkViewOther.Object;
            _carOther = new Car(_autoBehaviourOther) { CarObject = { NetworkView = NetworkViewOther.Object } };

            _gameObjectCountDownController = GameObject.CreatePrimitive(PrimitiveType.Cube);
            _countdownController = _gameObjectCountDownController.AddComponent<CountdownController>();

			MainScript.SelfPlayer = new Player { Role = new Driver() };
			MainScript.SelfCar = _carDriver;
        }

        [TearDown]
        public void Clear()
        {
            Utils.DestroyObject(_gameObject);
            Utils.DestroyObject(_gameObjectOther);
            Utils.DestroyObject(_gameObjectCountDownController);
            InputWrapper.Clear();
        }

        [Test]
        public void Test_SendToOther()
        {
            var pos = _autoBehaviour.transform.position;

            _carOther.CarObject.transform.position = pos;
            _player.Role.SendToOther(_carOther);

            Assert.AreEqual(MathUtils.Copy(pos), _throttler.GetLastSentPosition());
            NetworkViewOther.Verify(
                net =>
                    net.RPC(It.IsAny<string>(), It.IsAny<RPCMode>(), It.IsAny<Vector3>(), It.IsAny<float>(),
                        It.IsAny<int>()));
        }

        [Test]
        public void Test_GetPlayerAction_NotAllowedToDrive()
        {
            _countdownController.CountDownValue = 10;
            MainScript.CountdownController = _countdownController;

            PlayerAction action = _throttler.GetPlayerAction();

            Assert.AreEqual(PlayerAction.None, action);
        }

        [Test]
        public void Test_GetPlayerAction_None()
        {
            var action = _throttler.GetPlayerAction();
            Assert.AreEqual(PlayerAction.None, action);
        }

        [Test]
        public void Test_GetPlayerAction_UpArrow()
        {
            _countdownController.CountDownValue = -1;
            MainScript.CountdownController = _countdownController;

            InputWrapper.SetKey(KeyCode.UpArrow, true);
            var paNew = _throttler.GetPlayerAction();

            Assert.AreEqual(PlayerAction.SpeedUp, paNew);
        }

        [Test]
        public void Test_GetPlayerAction_DownArrow()
        {
            _countdownController.CountDownValue = -1;
            MainScript.CountdownController = _countdownController;
            MainScript.AmountPlayersConnected = GameData.PLAYERS_AMOUNT;

            InputWrapper.SetKey(KeyCode.DownArrow, true);
            var paNew = _throttler.GetPlayerAction();

            Assert.AreEqual(PlayerAction.SpeedDown, paNew);
        }

        [Test]
        public void Test_GetPlayerAction_TouchRight()
        {
            _countdownController.CountDownValue = -1;
            MainScript.CountdownController = _countdownController;
            MainScript.AmountPlayersConnected = GameData.PLAYERS_AMOUNT;

            var paPrev = _throttler.GetPlayerAction();
            InputWrapper.SetTouchCount(1);
            InputWrapper.SetTouch(0, new Vector2(_midScreenWidth + 10.0f, y: 1.0f));
            var paNew = _throttler.GetPlayerAction();

            Assert.AreNotEqual(paPrev, paNew);
            Assert.AreEqual(PlayerAction.SpeedUp, paNew);
        }

        [Test]
        public void Test_GetPlayerAction_TouchLeft()
        {
            _countdownController.CountDownValue = -1;
            MainScript.CountdownController = _countdownController;
            MainScript.AmountPlayersConnected = GameData.PLAYERS_AMOUNT;

            var paPrev = _throttler.GetPlayerAction();
            InputWrapper.SetTouchCount(1);
            InputWrapper.SetTouch(0, new Vector2(_midScreenWidth - 10.0f, 1.0f));
            var paNew = _throttler.GetPlayerAction();

            Assert.AreNotEqual(paPrev, paNew);
            Assert.AreEqual(PlayerAction.SpeedDown, paNew);
        }

        [Test]
        public void Test_HandlePlayerAction_applySpeedUp()
        {
            // Variables used
            const float speed = -0.01f;
            const int backwardAccelarationFactor = 10;
            MainScript.AmountPlayersConnected = GameData.PLAYERS_AMOUNT;

            InputWrapper.SetKey(KeyCode.UpArrow, true);
            _autoBehaviour.Speed = speed;

            _throttler.HandlePlayerAction(_autoBehaviour);
            var newSpeed = speed + backwardAccelarationFactor * _autoBehaviour.Acceleration * Time.deltaTime;

            Assert.AreEqual(newSpeed, _autoBehaviour.Speed);
        }

        [Test]
        public void Test_HandlePlayerAction_applySpeedDown()
        {
            _countdownController.CountDownValue = -1;
            MainScript.CountdownController = _countdownController;
            MainScript.AmountPlayersConnected = GameData.PLAYERS_AMOUNT;

            // Variables used
            const float speed = 0.01f;
            const int forwardAccelarationFactor = 20;

            InputWrapper.SetKey(KeyCode.DownArrow, true);
            _autoBehaviour.Speed = speed;

            _throttler.HandlePlayerAction(_autoBehaviour);
            var newSpeed = speed + forwardAccelarationFactor * _autoBehaviour.Acceleration * Time.deltaTime;

            Assert.AreEqual(newSpeed, _autoBehaviour.Speed, Delta);
        }

        [Test]
        public void Test_HandlePlayerAction_applyFrictionPositive()
        {
            // Variables used
            const float speed = 5.0f;

            // Assign variables
            _autoBehaviour.Speed = speed;

            // Trigger the method
            _throttler.HandlePlayerAction(_autoBehaviour);

            // Assertions
            Assert.AreEqual(speed, _autoBehaviour.Speed, Delta);
        }

        [Test]
        public void Test_HandlePlayerAction_applyFrictionNegative()
        {
            // Variables used
            const float speed = -5.0f;

            // Assign variables
            _autoBehaviour.Speed = speed;

            // Trigger the method
            _throttler.HandlePlayerAction(_autoBehaviour);

            // Assertions
            Assert.AreEqual(speed, _autoBehaviour.Speed, Delta);
        }

        [Test]
        public void Test_HandlePlayerAction_applyFrictionZero()
        {
            // Variables used
            const float speed = 0.0f;

            // Assign variables
            _autoBehaviour.Speed = speed;

            // Trigger the method
            _throttler.HandlePlayerAction(_autoBehaviour);

            // Assertions
            Assert.AreEqual(speed, _autoBehaviour.Speed);
        }

        //        [Test]
        //        public void Test_HandleCollision_NotMud()
        //        {
        //            const float speed = 0.1f;
        //
        //            var newSpeed = -(speed + Mathf.Sign(speed) * GameData.COLLISION_CONSTANT) * GameData.COLLISION_FACTOR;
        //            var rot = _autoBehaviour.transform.rotation;
        //            var pos = _autoBehaviour.transform.position;
        //
        //            var gameObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
        //            Utils.DestroyObject(gameObject.GetComponent<BoxCollider>());
        //            var colliderGameObject = gameObject.AddComponent<BoxCollider2D>();
        //            colliderGameObject.gameObject.tag = "Untagged";
        //
        //            _autoBehaviour.Speed = speed;
        //
        //            _throttler.HandleCollision(_autoBehaviour, colliderGameObject);
        //
        //            Assert.AreEqual(newSpeed, _autoBehaviour.Speed);
        //            Assert.AreEqual(rot, _autoBehaviour.transform.rotation);
        //            Assert.AreEqual(pos, _autoBehaviour.transform.position);
        //
        //            Utils.DestroyObject(gameObject);
        //        }
        /*
        [Test]
        public void Test_HandleCollision_Mud()
        {
            const float speed = 5.0f;
            const float expectedSpeed = 0;
            var rot = _autoBehaviour.transform.rotation;
            var pos = _autoBehaviour.transform.position;

            var gameObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
            Utils.DestroyObject(gameObject.GetComponent<BoxCollider>());
            var colliderGameObject = gameObject.AddComponent<BoxCollider2D>();
            colliderGameObject.gameObject.tag = "Mud";

            _autoBehaviour.Speed = speed;

            _throttler.HandleCollision(_autoBehaviour, colliderGameObject);

            Assert.AreEqual(expectedSpeed, _autoBehaviour.Speed);
            Assert.AreEqual(rot, _autoBehaviour.transform.rotation);
            Assert.AreEqual(pos, _autoBehaviour.transform.position);

            Utils.DestroyObject(gameObject);
        }*/
    }
}