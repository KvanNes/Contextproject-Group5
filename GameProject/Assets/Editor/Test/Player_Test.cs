using UnityEngine;
using NUnit.Framework;
using Moq;
using System;

[TestFixture]
public class PlayerTest
{
    private bool initialized = false;

    private Player player { get; set; }
    private Player playerPar { get; set; }
    private PlayerRole driverRole { get; set; }
    private Car car { get; set; }

    [TestFixtureSetUp]
    public void SetUp()
    {
        if (!initialized)
        {
            player = new Player();

            driverRole = new Driver();
            car = new Car();
            playerPar = new Player(car, driverRole);
        }
    }

    [SetUp]
    public void InitTest()
    {
        player = new Player();

        driverRole = new Driver();
        car = new Car();
        playerPar = new Player(car, driverRole);
    }

    [Test]
    public void TestConstructor()
    {
        Assert.IsNull(player.Car);
        Assert.IsNull(player.Role);
    }

    public void TestConstructorNotNull()
    {
        Assert.IsNotNull(playerPar);
    }

    [Test]
    public void TestConstructor_Par()
    {
        Assert.AreEqual(car, playerPar.Car);
        Assert.AreEqual(driverRole, playerPar.Role);
    }
}
