using Behaviours;
using Cars;
using NetworkManager;
using UnityEngine;
using NUnit.Framework;
using System.Collections.Generic;
using Utilities;

[TestFixture]
public class GameTest
{

    private Game _game;

    private Car _car;
    private GameObject _gameObject;
    private AutoBehaviour _autoBehaviour;

    private Car _carOther;
    private GameObject _gameObjectOther;

    [SetUp]
    public void SetUp()
    {
        _game = new Game();

        _gameObject = Object.Instantiate(Resources.LoadAssetAtPath("Assets/Auto.prefab", typeof(GameObject))) as GameObject;
        if (_gameObject == null) return;
        _autoBehaviour = _gameObject.AddComponent<AutoBehaviour>();
        _car = new Car(_autoBehaviour);

        _gameObjectOther = Object.Instantiate(Resources.LoadAssetAtPath("Assets/Auto.prefab", typeof(GameObject))) as GameObject;
        if (_gameObjectOther == null) return;
        _gameObjectOther.AddComponent<AutoBehaviour>();
        _carOther = new Car(_gameObjectOther.GetComponent<AutoBehaviour>());

        List<Car> cars = new List<Car>();
        for (int i = 0; i < GameData.CARS_AMOUNT; i++)
        {
            Car c = new Car(_autoBehaviour);
            cars.Add(c);
        }
        MainScript.Cars = cars;
        MainScript.SelfCar = new Car {CarObject = _autoBehaviour};
    }

    [TearDown]
    public void Clear()
    {
        Utils.DestroyObject(_gameObject);
        Utils.DestroyObject(_gameObjectOther);
    }

    [Test]
    public void Test_ConstructorVariables()
    {
        Assert.IsNotNull(_game.SpawnController);
        Assert.IsNotNull(_game.AvailabilityController);
        Assert.IsNotNull(_game.StartController);
        Assert.IsNotNull(_game.Cars);
    }

    [Test]
    public void Test_CarsEmpty()
    {
        Assert.IsEmpty(_game.Cars);
    }

    [Test]
    public void Test_addCar()
    {
        const int carNumber = 0;

        _game.AddCar(_car);

        Assert.IsNotEmpty(_game.Cars);
        Assert.AreEqual(carNumber, _game.Cars[0].CarObject.CarNumber);
    }

    [Test]
    public void Test_addCarTwice()
    {
        const int carNumber = 0;

        _game.AddCar(_car);
        _game.AddCar(_carOther);

        Assert.IsNotEmpty(_game.Cars);
        Assert.AreEqual(carNumber, _game.Cars[0].CarObject.CarNumber);
        Assert.AreEqual(1, _game.Cars[1].CarObject.CarNumber);
    }
}
