using GraphicalUI;
using NUnit.Framework;

namespace GraphicalUITests
{
    public class PartsConfigurationTest
    {

        private PartsConfiguration _partsConfiguration;
        private GraphicalUIPart[] _parts;

        private MainPart _mainPart;
        private ThrottlerPart _throttlerPart;

        [Test]
        public void Test_Constructor()
        {
            _mainPart = new MainPart();
            _throttlerPart = new ThrottlerPart();
            _parts = new GraphicalUIPart[] { _mainPart, _throttlerPart };

            _partsConfiguration = new PartsConfiguration(_parts);

            Assert.AreEqual(2, _partsConfiguration.Parts.Count);

            GraphicalUIPart[] parts = new GraphicalUIPart[_partsConfiguration.Parts.Count];
            _partsConfiguration.Parts.CopyTo(parts);
            for (int i = 0; i < parts.Length; i++)
            {
                Assert.AreEqual(_parts[i], parts[i]);
            }
        }
    }
}