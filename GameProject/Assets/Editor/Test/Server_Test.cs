using Behaviours;
using Cars;
using Mock;
using NetworkManager;
using UnityEngine;
using NUnit.Framework;
using Moq;
using System.Collections.Generic;
using Utilities;

[TestFixture]
public class ServerTest
{
    private Server _testServer;
    private GameObject _gameObject;
    private GameObject _networkManagerGameObject;
    private GameObject _playerPrefabGameObject;
    private AutoBehaviour _carObject;
    private NetworkPlayer _networkPlayer;

    public Mock<INetwork> Network;
    public Mock<INetworkView> NetworkView;

    private const string TypeStringEmpty = "";
    private const string TypeStringThrottler = "Throttler";
    private const string TypeStringDriver = "Driver";
    private const int CarNumberInit = 0;
    private const int CarNumberNegative = -1;
    private const int CarNumberTen = 10;

    /*
     * Setup for the tests by creating the appropiate mocks and setting up the server
    */
    [SetUp]
    public void SetupServer()
    {
        _networkPlayer = new NetworkPlayer();
        _gameObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
        _testServer = _gameObject.AddComponent<Server>();
        _carObject = _gameObject.AddComponent<AutoBehaviour>();
        Network = new Mock<INetwork>();
        NetworkView = new Mock<INetworkView>();
        _testServer.Game = new Game();
        _testServer.Network = Network.Object;
        _testServer.StartServer();
        _testServer.NetworkView = NetworkView.Object;
        _carObject.NetworkView = NetworkView.Object;

        _networkManagerGameObject = GameObject.FindGameObjectWithTag("Network");
        _testServer.PlayerPrefab = _networkManagerGameObject.GetComponent<Server>().PlayerPrefab;
        _testServer.SpawnObject = _networkManagerGameObject.GetComponent<Server>().transform;

        var cars = new List<Car>();
        for (var i = 0; i < GameData.CARS_AMOUNT; i++)
        {
            var c = new Car(_carObject);
            cars.Add(c);
        }
        MainScript.Cars = cars;
    }

    [TearDown]
    public void Clear()
    {
        Utils.DestroyObject(_gameObject);
        _testServer.Game.Cars.Clear();
        _testServer.DisconnectServer();
        Utils.DestroyObjects(GameObject.FindGameObjectsWithTag("Player"));
    }

    /**
     * Test rather the server has made an InitializeServer call
    */
    [Test]
    public void TestInitialization()
    {
        _testServer.Network = Network.Object;
        _testServer.StartServer();
        Network.Verify(net => net.InitializeServer(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<bool>()));
    }

    [Test]
    public void TestConnected()
    {
        Assert.IsTrue(_testServer.IsConnected());
    }

    [Test]
    public void TestDisconnect()
    {
        _testServer.DisconnectServer();
        Assert.IsFalse(_testServer.IsConnected());
    }

    [Test]
    public void Test_noJobAvailable_NegativeCarNum()
    {
        var res = _testServer.checkJobAvailableAndMaybeAdd(TypeStringEmpty, CarNumberNegative, _networkPlayer);
        Assert.IsFalse(res);
    }

    [Test]
    public void Test_checkJobAvailable_OverloadCarNum()
    {
        var res = _testServer.checkJobAvailableAndMaybeAdd(TypeStringEmpty, CarNumberTen, _networkPlayer);
        Assert.IsFalse(res);
    }

    [Test]
    public void Test_checkJobAvailable_CarNull()
    {
        var c1 = new Car(0) { CarObject = _carObject };
        var c2 = new Car(1) { CarObject = _carObject };

        _testServer.Game.AddCar(c1);
        _testServer.Game.AddCar(c2);
        _testServer.Game.Cars[0] = null;

        var res = _testServer.checkJobAvailableAndMaybeAdd(TypeStringEmpty, 0, _networkPlayer);
        Assert.IsFalse(res);
    }

    [Test]
    public void Test_checkJobAvailable_NetworkPlayerThrottler()
    {
        var c1 = new Car(CarNumberInit)
        {
            CarObject = _carObject,
            Throttler = new Player { NetworkPlayer = UnityEngine.Network.player }
        };

        _testServer.Game.AddCar(c1);

        var res = _testServer.checkJobAvailableAndMaybeAdd(TypeStringThrottler, CarNumberInit, _networkPlayer);
        Assert.IsFalse(res);
    }

    [Test]
    public void Test_checkJobAvailable_NetworkPlayerDriver()
    {
        var c1 = new Car(CarNumberInit)
        {
            CarObject = _carObject,
            Driver = new Player { NetworkPlayer = UnityEngine.Network.player }
        };

        _testServer.Game.AddCar(c1);

        var res = _testServer.checkJobAvailableAndMaybeAdd(TypeStringDriver, CarNumberInit, _networkPlayer);
        Assert.IsFalse(res);
    }

    [Test]
    public void Test_checkJobAvailable_Throttler()
    {
        var c1 = new Car(CarNumberInit) { CarObject = _carObject, Throttler = new Player() };

        _testServer.Game.AddCar(c1);
        _testServer.checkJobAvailableAndMaybeAdd(TypeStringThrottler, CarNumberInit, _networkPlayer);

        Assert.AreEqual(_networkPlayer, _testServer.Game.Cars[CarNumberInit].Throttler.NetworkPlayer);
    }

    [Test]
    public void Test_checkJobAvailable_Driver()
    {
        var c1 = new Car(CarNumberInit) { CarObject = _carObject, Driver = new Player() };

        _testServer.Game.AddCar(c1);
        _testServer.checkJobAvailableAndMaybeAdd(TypeStringDriver, CarNumberInit, _networkPlayer);

        Assert.AreEqual(_networkPlayer, _testServer.Game.Cars[CarNumberInit].Driver.NetworkPlayer);
    }

    [Test]
    public void Test_checkJobAvailable()
    {
        var c1 = new Car(CarNumberInit) { CarObject = _carObject, Driver = new Player() };

        _testServer.Game.AddCar(c1);

        var res = _testServer.checkJobAvailableAndMaybeAdd(TypeStringEmpty, CarNumberInit, _networkPlayer);
        Assert.IsTrue(res);
    }

    [Test]
    public void Test_ChooseJob_JobAvailable()
    {
        var c1 = new Car(CarNumberInit) { CarObject = _carObject, Driver = new Player() };
        _testServer.Game.AddCar(c1);
        _testServer.chooseJob(TypeStringEmpty, CarNumberTen, default(NetworkMessageInfo));

        NetworkView.Verify(net => net.RPC(It.IsAny<string>(), It.IsAny<NetworkPlayer>()));
    }

    [Test]
    public void Test_ChooseJob_JobNotAvailable()
    {
        _testServer.chooseJob(TypeStringEmpty, CarNumberTen, default(NetworkMessageInfo));

        NetworkView.Verify(net => net.RPC(It.IsAny<string>(), It.IsAny<NetworkPlayer>()));
    }

    [Test]
    public void Test_GetStartingPosition()
    {
        const float expectedPosition = 0.12f - 0.05f * CarNumberInit;
        var startingPosition = Server.GetStartingPosition(CarNumberInit);

        Assert.AreEqual(expectedPosition, startingPosition);
    }

    [Test]
    public void Test_OnServerInitialized_Game()
    {
        _testServer.Network = new NetworkWrapper();
        _testServer.StartServer();

        _testServer.OnServerInitialized();

        Assert.IsNotNull(_testServer.Game);
    }

    [Test]
    public void Test_OnServerInitialized_Camera()
    {
        Vector3 expected = new Vector3(0, 0, 10);
        _testServer.Network = new NetworkWrapper();
        _testServer.StartServer();

        _testServer.OnServerInitialized();

        Assert.AreEqual(expected, Camera.main.transform.position);
    }

    [Test]
    public void Test_OnServerInitialized_TwoCarsAdded()
    {
        const int expected = 2;
        _testServer.Network = new NetworkWrapper();
        _testServer.StartServer();

        _testServer.OnServerInitialized();

        Assert.AreEqual(expected, _testServer.Game.Cars.Count);
    }

    [Test]
    public void Test_OnServerInitialized_TwoDriversTwoThrottlers()
    {
        _testServer.Network = new NetworkWrapper();
        _testServer.StartServer();

        _testServer.OnServerInitialized();

        foreach (Car c in _testServer.Game.Cars)
        {
            Assert.IsNotNull(c.Driver);
            Assert.IsTrue(c.Driver.Role.GetType() == typeof(Driver));
            Assert.IsNotNull(c.Throttler);
            Assert.IsTrue(c.Throttler.Role.GetType() == typeof(Throttler));
        }
    }
}