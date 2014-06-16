using GraphicalUI;
using NUnit.Framework;
using UnityEngine;

namespace GraphicalUITests
{
    public class GraphicalUIControllerTest
    {
        private GameObject _gameObject;

        private GraphicalUIController _graphicalUiController;
        private PartsConfiguration _partsConfigurationMain;
        private PartsConfiguration _partsConfigurationTutorial;

        [SetUp]
        public void SetUp()
        {
            _gameObject = GameObject.CreatePrimitive(PrimitiveType.Cube);

            _graphicalUiController = _gameObject.AddComponent<GraphicalUIController>();
            _partsConfigurationMain = new PartsConfiguration(new MainPart());
            _partsConfigurationTutorial = new PartsConfiguration(new TutorialPart());
        }

        [TearDown]
        public void Clear()
        {
            _graphicalUiController.Clear();
        }

        [Test]
        public void Test_Add()
        {
            _graphicalUiController.Add(_partsConfigurationMain);

            Assert.AreEqual(_partsConfigurationMain, _graphicalUiController.Configurations.Peek());
        }

        [Test]
        public void Test_Remove()
        {
            _graphicalUiController.Add(_partsConfigurationMain);
            _graphicalUiController.Remove();

            Assert.IsEmpty(_graphicalUiController.Configurations);
        }

        [Test]
        public void Test_Clear()
        {
            _graphicalUiController.Add(_partsConfigurationMain);
            _graphicalUiController.Add(_partsConfigurationTutorial);

            _graphicalUiController.Clear();

            Assert.IsEmpty(_graphicalUiController.Configurations);
        }
    }
}