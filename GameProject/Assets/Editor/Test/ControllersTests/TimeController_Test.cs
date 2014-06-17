using Controllers;
using NUnit.Framework;
using UnityEngine;

namespace ControllersTests
{
    [TestFixture]
    public class TimeControllerTest
    {
        private TimeController _timeController;

        private const float Delta = 0.1f;

        [SetUp]
        public void SetUp()
        {
            _timeController = TimeController.GetInstance();
        }

        [TearDown]
        public void Clear()
        {
            _timeController.Clear();
        }

        [Test]
        public void Test_Initial()
        {
            _timeController = TimeController.GetInstance();

            Assert.IsNotNull(_timeController);
        }

        [Test]
        public void Test_ResetTimer()
        {
            _timeController.ResetTimer();

            Assert.AreEqual(Network.time, _timeController.GetStartTime());
        }

        [Test]
        public void Test_StopTimer()
        {
            _timeController.StopTimer();

            Assert.AreEqual(Network.time, _timeController.GetStopTime());
        }

        [Test]
        public void Test_ResetStopTime()
        {
            const double expected = -1;

            _timeController.ResetStopTime();

            Assert.AreEqual(expected, _timeController.GetStopTime());
        }

        [Test]
        public void Test_GetTime_Negative()
        {
            const double expected = 0;
            double result = _timeController.GetTime();

            Assert.AreEqual(expected, result, Delta);
        }

        [Test]
        public void Test_GetTime_Positive()
        {
            _timeController.SetStopTime(10);

            double expected = _timeController.GetStopTime() - _timeController.GetStartTime();
            double result = _timeController.GetTime();

            Assert.AreEqual(expected, result);
        }
    }
}
