using Behaviours;
using Cars;
using UnityEngine;
using NUnit.Framework;
using Utilities;
using Wrappers;

namespace CarsTests
{
    [TestFixture]
    public class ActionTest
    {
        private PlayerType _throttlerPlayerType = PlayerType.Throttler;
        private PlayerType _driverPlayerType = PlayerType.Driver;
        private GameObject _gameObject;
        private CarBehaviour _carBehaviour;

        [SetUp]
        public void SetUp()
        {
            _gameObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
            _carBehaviour = _gameObject.AddComponent<CarBehaviour>();
        }

        [TearDown]
        public void Clear()
        {
            Utils.DestroyObject(_gameObject);
            InputWrapper.Reset();
        }

        [Test]
        public void Test_GetTouchAction_Driver1()
        {
            InputWrapper.SetTouchCount(0);

            Assert.AreEqual(PlayerAction.None, Action.GetTouchAction(_driverPlayerType));
        }

        [Test]
        public void Test_GetTouchAction_Driver2()
        {
            InputWrapper.SetTouchCount(10);
            InputWrapper.SetTouch(0, new Vector2(Screen.width / 2 - 50, 20));

            Assert.AreEqual(PlayerAction.SteerLeft, Action.GetTouchAction(_driverPlayerType));
        }

        [Test]
        public void Test_GetTouchAction_Driver3()
        {
            InputWrapper.SetTouchCount(10);
            InputWrapper.SetTouch(0, new Vector2(Screen.width / 2 + 50, 20));

            Assert.AreEqual(PlayerAction.SteerRight, Action.GetTouchAction(_driverPlayerType));
        }

        [Test]
        public void Test_GetTouchAction_Throttler1()
        {
            InputWrapper.SetTouchCount(0);

            Assert.AreEqual(PlayerAction.None, Action.GetTouchAction(_throttlerPlayerType));
        }

        [Test]
        public void Test_GetTouchAction_Throttler2()
        {
            InputWrapper.SetTouchCount(10);
            InputWrapper.SetTouch(0, new Vector2(Screen.width / 2 - 50, 20));

            Assert.AreEqual(PlayerAction.SpeedDown, Action.GetTouchAction(_throttlerPlayerType));
        }

        [Test]
        public void Test_GetTouchAction_Throttler3()
        {
            InputWrapper.SetTouchCount(10);
            InputWrapper.SetTouch(0, new Vector2(Screen.width / 2 + 50, 20));

            Assert.AreEqual(PlayerAction.SpeedUp, Action.GetTouchAction(_throttlerPlayerType));
        }

        [Test]
        public void Test_GetKeyboardAction_Driver1()
        {
            InputWrapper.SetKey(KeyCode.LeftArrow, true);

            Assert.AreEqual(PlayerAction.SteerLeft, Action.GetKeyboardAction(_driverPlayerType));
        }

        [Test]
        public void Test_GetKeyboardAction_Driver2()
        {
            InputWrapper.SetKey(KeyCode.RightArrow, true);

            Assert.AreEqual(PlayerAction.SteerRight, Action.GetKeyboardAction(_driverPlayerType));
        }

        [Test]
        public void Test_GetKeyboardAction_Driver3()
        {
            Assert.AreEqual(PlayerAction.None, Action.GetKeyboardAction(_driverPlayerType));
        }

        [Test]
        public void Test_GetKeyboardAction_Throttler1()
        {
            InputWrapper.SetKey(KeyCode.DownArrow, true);

            Assert.AreEqual(PlayerAction.SpeedDown, Action.GetKeyboardAction(_throttlerPlayerType));
        }

        [Test]
        public void Test_GetKeyboardAction_Throttler2()
        {
            InputWrapper.SetKey(KeyCode.UpArrow, true);

            Assert.AreEqual(PlayerAction.SpeedUp, Action.GetKeyboardAction(_throttlerPlayerType));
        }

        [Test]
        public void Test_GetKeyboardAction_Throttler3()
        {
            Assert.AreEqual(PlayerAction.None, Action.GetKeyboardAction(_throttlerPlayerType));
        }

        [Test]
        public void Test_GetAccelerationIncrease_Increase()
        {
            Assert.AreEqual(GameData.ACCELERATION_INCREASE, Action.GetAccelerationIncrease(PlayerAction.SpeedUp));
        }

        [Test]
        public void Test_GetAccelerationIncrease_Decrease()
        {
            Assert.AreEqual(GameData.ACCELERATION_DECREASE, Action.GetAccelerationIncrease(PlayerAction.SpeedDown));
        }

        [Test]
        public void Test_GetRotationSpeedFactor_None()
        {
            Assert.AreEqual(0, Action.GetAccelerationIncrease(PlayerAction.None));
        }

        [Test]
        public void Test_GetRotationSpeedFactor_Increase()
        {
            Assert.AreEqual(GameData.ROTATION_SPEED_FACTOR, Action.GetRotationSpeedFactor(PlayerAction.SteerLeft));
        }

        [Test]
        public void Test_GetRotationSpeedFactor_Decrease()
        {
            Assert.AreEqual(-GameData.ROTATION_SPEED_FACTOR, Action.GetRotationSpeedFactor(PlayerAction.SteerRight));
        }

        [Test]
        public void Test_GetAccelerationIncrease_None()
        {
            Assert.AreEqual(0, Action.GetAccelerationIncrease(PlayerAction.None));
        }   

        [Test]
        public void Test_GetAccelerationFactor_SpeedUp1()
        {
            const int expected = 10;
            _carBehaviour.Speed = -1;

            Assert.AreEqual(expected, Action.GetAccelerationFactor(PlayerAction.SpeedUp, _carBehaviour));
        }
        [Test]
        public void Test_GetAccelerationFactor_SpeedUp2()
        {
            const int expected = 5;
            _carBehaviour.Speed = 1;

            Assert.AreEqual(expected, Action.GetAccelerationFactor(PlayerAction.SpeedUp, _carBehaviour));
        }
        [Test]
        public void Test_GetAccelerationFactor_SpeedDown1()
        {
            const int expected = 10;
            _carBehaviour.Speed = -1;

            Assert.AreEqual(expected, Action.GetAccelerationFactor(PlayerAction.SpeedDown, _carBehaviour));
        }

        [Test]
        public void Test_GetAccelerationFactor_SpeedDown2()
        {
            const int expected = 50;
            _carBehaviour.Speed = 1;

            Assert.AreEqual(expected, Action.GetAccelerationFactor(PlayerAction.SpeedDown, _carBehaviour));
        }

        [Test]
        public void Test_GetAccelerationFactor_None()
        {
            const int expected = 0;

            Assert.AreEqual(expected, Action.GetAccelerationFactor(PlayerAction.None, _carBehaviour));
        }
    }
}