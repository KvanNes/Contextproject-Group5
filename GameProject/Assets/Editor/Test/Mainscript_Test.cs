using UnityEngine;
using NUnit.Framework;
using Moq;

[TestFixture]
public class MainscriptTest
{
    private MainScript _mainScript;
    private GameObject _gameObject;
    private GameObject _gObjNetwork;

    private Mock<ICar> _selfCar;

    [SetUp]
    public void SetUp()
    {
        _gameObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
        _gObjNetwork = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        _gObjNetwork.AddComponent<Server>();
        _gObjNetwork.GetComponent<Server>().tag = "Network";
        _gObjNetwork.AddComponent<Client>();
        _gObjNetwork.GetComponent<Client>().tag = "Network";
        _gObjNetwork.tag = "Network";

        _mainScript = _gameObject.AddComponent<MainScript>();
        _selfCar = new Mock<ICar>();

        _mainScript.SetSelfCar(_selfCar.Object);
        _mainScript.Initialize();
    }

    [TearDown]
    public void Cleanup()
    {
        Utils.DestroyObject(_gameObject);
        Utils.DestroyObject(_gObjNetwork);
    }

    [Test]
    public void Test_ServerSet()
    {
        Assert.IsNotNull(_mainScript.GetServer());
    }

    [Test]
    public void Test_ClientSet()
    {
        Assert.IsNotNull(_mainScript.GetClient());
    }

    [Test]
    public void Test_ServerNetwork()
    {
        //Assert.IsNotNull(_mainScript.GetServer().Network);
    }

    [Test]
    public void Test_Cars()
    {
        Assert.IsNotNull(_mainScript.GetCars());
        Assert.AreEqual(GameData.CARS_AMOUNT, _mainScript.GetCars().Count);
    }

    [Test]
    public void Test_SendToOther()
    {
        _mainScript.SendToOther();
        _selfCar.Verify(car => car.SendToOther());
    }

}
