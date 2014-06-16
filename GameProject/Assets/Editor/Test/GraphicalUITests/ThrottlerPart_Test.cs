using GraphicalUI;
using NUnit.Framework;

namespace GraphicalUITests
{
    public class ThrottlerPartTest
    {

        private ThrottlerPart _throttlerPart;

        [SetUp]
        public void SetUp()
        {
            _throttlerPart = new ThrottlerPart();
        }

        [Test]
        public void Test_Initialize()
        {
            _throttlerPart.Initialize();

            Assert.IsNotNull(_throttlerPart.TextureNormal);
            Assert.IsNotNull(_throttlerPart.TexturePressed);
        }
    }
}