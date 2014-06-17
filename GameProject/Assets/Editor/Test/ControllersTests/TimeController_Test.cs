using Controllers;
using Interfaces;
using Moq;
using NUnit.Framework;
using UnityEngine;
using Utilities;
using Wrappers;

namespace ControllersTests
{
    [TestFixture]
    public class TimeControllerTest
    {
        private TimeController _timeController;

        private Mock<INetwork> _networkMock;

        [SetUp]
        public void SetUp()
        {
            _timeController = TimeController.GetInstance();
            _networkMock = new Mock<INetwork>();
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
            _timeController.Network = _networkMock.Object;
            _timeController.ResetTimer();

            _networkMock.Verify(net => net.GetTime());
        }

        [Test]
        public void Test_StopTimer()
        {
            _timeController.Network = _networkMock.Object;
            _timeController.StopTimer();

            _networkMock.Verify(net => net.GetTime());
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
            _timeController.Network = _networkMock.Object;

            _timeController.GetTime();

            _networkMock.Verify(net => net.GetTime());
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
