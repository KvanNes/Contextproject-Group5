using UnityEngine;
using NUnit.Framework;

[TestFixture]
public class CarTest
{
    private const int Zero = 0;
    private const int One = 1;

    private GameObject gameObject;

    private Car _car;
    private Car _carAutoBehaviour;
    private AutoBehaviour _autoBehaviour;

    private Player _driver;
    private Player _throttler;

    private PlayerRole _driverRole;
    private PlayerRole _throttlerRole;

    [SetUp]
    public void SetUp()
    {
        _car = new Car();
        gameObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
        _autoBehaviour = gameObject.AddComponent<AutoBehaviour>();
        _carAutoBehaviour = new Car(_autoBehaviour);

        _driverRole = new Driver();
        _throttlerRole = new Throttler();

        _driver = new Player();
        _throttler = new Player();

        _driver.Role = _driverRole;
        _throttler.Role = _throttlerRole;
    }

    [TearDown]
    public void Cleanup()
    {
        Utils.DestroyObject(gameObject);
        _car.Clear();
        _carAutoBehaviour.Clear();
    }

    [Test]
    public void TestConstructor1_NotNull()
    {
        Assert.IsNotNull(_car);
    }

    [Test]
    public void TestConstructor1_Variables1()
    {
        Assert.IsNull(_car.CarObject);
    }

    [Test]
    public void TestConstructor1_Variables2()
    {
        Assert.AreEqual(One, _car.carNumber);
    }

    [Test]
    public void TestConstructor2_NotNull()
    {
        Assert.IsNotNull(_carAutoBehaviour);
    }

    [Test]
    public void TestConstructor2_ParNull()
    {
        Assert.IsNotNull(_carAutoBehaviour.CarObject);
    }

    [Test]
    public void TestClearCar()
    {
        _car.Clear();

        Assert.IsNull(_car.Driver);
        Assert.IsNull(_car.Throttler);
        Assert.IsNull(_car.CarObject);
        Assert.AreEqual(Zero, _car.GetAmountPlayers());
        Assert.AreEqual(Zero, _car.carNumber);
    }


}
