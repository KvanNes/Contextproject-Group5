using GraphicalUI;
using NUnit.Framework;

namespace GraphicalUITests
{
    public class DriverPartTest
    {

        private DriverPart _driverPart;

        [SetUp]
        public void SetUp()
        {
            _driverPart = new DriverPart();
        }

        [Test]
        public void Test_Initialize()
        {
            _driverPart.Initialize();

            Assert.IsNotNull(_driverPart.TextureLeftNormal);
            Assert.IsNotNull(_driverPart.TextureLeftPressed);
            Assert.IsNotNull(_driverPart.TextureRightNormal);
            Assert.IsNotNull(_driverPart.TextureLeftPressed);
        }
    }
}
