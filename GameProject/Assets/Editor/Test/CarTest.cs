using UnityEngine;
using NUnit.Framework;
using Moq;
using System;

[TestFixture]
public class CarTest : MonoBehaviour
{
    private readonly int ZERO = 0;
    private readonly int ONE = 1;

    private int initialCarNumber;

    private Car car;
    private Car car_ab;
    private AutoBehaviour ab;

    private Player driver;
    private Player throttler;

    private PlayerRole driverRole;
    private PlayerRole throttlerRole;

    [SetUp]
    public void BeforeTest()
    {
        car = new Car();

        ab = new AutoBehaviour();
        car_ab = new Car(ab);

        driverRole = new Driver();
        throttlerRole = new Throttler();

        driver = new Player();
        throttler = new Player();

        driver.Role = driverRole;
        throttler.Role = throttlerRole;

        initialCarNumber = car.carNumber;
        
    }

    [Test]
    public void TestConstructor1_NotNull()
    {
        Assert.IsNotNull(car);
    }

    [Test]
    public void TestConstructor1_Variables1()
    {
        Assert.IsNull(car.CarObject);
    }

    [Test]
    public void TestConstructor1_Variables2()
    {
        car = new Car(initialCarNumber);
        Assert.AreEqual(ONE, car.carNumber);
    }

    [Test]
    public void TestConstructor2_NotNull()
    {
        Assert.IsNotNull(car_ab);
    }

    [Test]
    public void TestConstructor2_ParNotNull()
    {
        Assert.IsNotNull(car_ab.CarObject);
    }

    [Test]
    public void TestAddPlayer_Driver()
    {
        car.addPlayer(driver);
        Assert.AreEqual(ONE, car.getAmountPlayers());
        Assert.AreEqual(driver, car.Driver);
        //Assert.AreEqual(car, car.Driver.getCar());
    }

    [Test]
    public void TestAddPlayer_Throttler()
    {
        car.addPlayer(driver);
        car.addPlayer(throttler);
        Assert.AreEqual(ONE + 1, car.getAmountPlayers());
        Assert.AreEqual(throttler, car.Throttler);
        //Assert.AreEqual(car, car.Throttler.GetCar());
    }

    [Test]
    [ExpectedException(typeof(UnityException))]
    public void TestAddPlayer_Fail()
    {
        Debug.Log(car.getAmountPlayers());
        car.addPlayer(driver);
        Debug.Log(car.getAmountPlayers());
        car.addPlayer(throttler);
        Debug.Log(car.getAmountPlayers());
        car.addPlayer(driver);
        Debug.Log(car.getAmountPlayers());
    }

    // TODO: Test SendToOther()

    [Test]
    public void TestAmountPlayers()
    {
        Assert.AreEqual(ZERO, car.getAmountPlayers());
        car.addPlayer(driver);
        Assert.AreEqual(ONE, car.getAmountPlayers());
    }


}
