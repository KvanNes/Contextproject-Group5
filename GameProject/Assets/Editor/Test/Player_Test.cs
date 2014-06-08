using Cars;
using NUnit.Framework;

[TestFixture]
public class PlayerTest
{

    private Player Player { get; set; }
    private Player PlayerPar { get; set; }
    private IPlayerRole DriverRole { get; set; }
    private Car Car { get; set; }

    [SetUp]
    public void SetUp()
    {
        Player = new Player();

        DriverRole = new Driver();
        Car = new Car();
        PlayerPar = new Player(Car, DriverRole);
    }

    [Test]
    public void TestConstructor()
    {
        Assert.IsNull(Player.Car);
        Assert.IsNull(Player.Role);
    }

    public void TestConstructorNotNull()
    {
        Assert.IsNotNull(PlayerPar);
    }

    [Test]
    public void TestConstructor_Par()
    {
        Assert.AreEqual(Car, PlayerPar.Car);
        Assert.AreEqual(DriverRole, PlayerPar.Role);
    }
}
