using Cars;
using Controllers;
using NetworkManager;
using NUnit.Framework;
using UnityEngine;

namespace ControllersTests
{
    public class NetworkControllerTest
    {

        private GameObject _gameObject;

        private NetworkController _networkController;

        [SetUp]
        public void SetUp()
        {
            _gameObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
            _networkController = _gameObject.AddComponent<NetworkController>();
        }

        [TearDown]
        public void Clear()
        {

        }

        [Test]
        public void Test_Start_Null()
        {
            MainScript.NetworkController = null;

            _networkController.Start();

            Assert.AreEqual(_networkController, MainScript.NetworkController);
        }

        [Test]
        [ExpectedException(typeof(Utilities.UnityException))]
        public void Test_Start_NotNull()
        {
            MainScript.NetworkController = _networkController;

            _networkController.Start();
        }

        [Test]
        public void Test_ToggleLight_White()
        {
            // Initialize for if-statement
            MainScript.SelfPlayer = new Player { Role = new Driver() };
            RenderSettings.ambientLight = Color.white;

            // Trigger the method
            _networkController.ToggleLight();

            Assert.AreEqual(Color.black, RenderSettings.ambientLight);
        }

        [Test]
        public void Test_ToggleLight_Other()
        {
            // Initialize for if-statement
            MainScript.SelfPlayer = new Player { Role = new Driver() };
            RenderSettings.ambientLight = Color.gray;

            // Trigger the method
            _networkController.ToggleLight();

            // Assertion
            Assert.AreEqual(Color.white, RenderSettings.ambientLight);
        }

        [Test]
        public void Test_ToggleOverview_FixedCamera()
        {
            // Expected value
            Quaternion expectedQuaternion = Quaternion.Euler(0, 180, 0);
            
            // Initialize first if-statement
            MainScript.SelfPlayer = new Player { Role = new Throttler() };

            // Initialize second if-statement
            MainScript.FixedCamera = false;

            // Trigger the method
            _networkController.ToggleOverview();

            // Assertion
            Assert.AreEqual(expectedQuaternion, Camera.main.transform.rotation);
        }

        [Test]
        public void Test_ToggleOverview_NotFixedCamera()
        {
            // Expected value
            Quaternion expectedQuaternion = Quaternion.Euler(0, 0, 0);

            // Initialize first if-statement
            MainScript.SelfPlayer = new Player { Role = new Throttler() };

            // Initialize second if-statement
            MainScript.FixedCamera = true;

            // Trigger the method
            _networkController.ToggleOverview();

            // Assertion
            Assert.AreEqual(expectedQuaternion, Camera.main.transform.rotation);
        }
    }
}
