using Controllers;
using Main;
using NUnit.Framework;
using UnityEngine;
using Utilities;

namespace ControllersTests
{
    public class CountdownControllerTest
    {

        private GameObject _gameObject;

        private CountdownController _countdownController;

        [SetUp]
        public void SetUp()
        {
            _gameObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
            _countdownController = _gameObject.AddComponent<CountdownController>();
        }

        [TearDown]
        public void Clear()
        {
            Utils.DestroyObject(_gameObject);
        }

        [Test]
        public void Test_Start()
        {
            const int expected = -100;

            _countdownController.Start();

            Assert.AreEqual(expected, _countdownController.CountDownValue);
        }

        [Test]
        public void Test_StartCountdown()
        {
            const int expected = 6;

            _countdownController.StartCountdown();

            Assert.AreEqual(expected, _countdownController.CountDownValue);
        }

        [Test]
        public void Test_StopCountdown()
        {
            const int expected = -100;

            _countdownController.Start();

            Assert.AreEqual(expected, _countdownController.CountDownValue);
        }

        [Test]
        public void Test_DecrementCounter1()
        {
            const int expected = -1;

            _countdownController.DecrementCounter();

            Assert.AreEqual(expected, _countdownController.CountDownValue);
        }

        [Test]
        public void Test_DecrementCounter3()
        {
            const int expected = -100;

            for (int i = 0; i < 3; i++)
            {
                _countdownController.DecrementCounter();
            }

            Assert.AreEqual(expected, _countdownController.CountDownValue);
        }

        [Test]
        public void Test_AllowedToDrive_True()
        {
            const bool expected = true;

            _countdownController.CountDownValue = -1;
            MainScript.AmountPlayersConnected = GameData.PLAYERS_AMOUNT;

            Assert.AreEqual(expected, _countdownController.AllowedToDrive());
        }

        [Test]
        public void Test_AllowedToDrive_False()
        {
            const bool expected = false;

            _countdownController.CountDownValue = -1;
            MainScript.AmountPlayersConnected = 0;

            Assert.AreEqual(expected, _countdownController.AllowedToDrive());
        }
    }
}
