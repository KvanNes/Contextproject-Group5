using System;
using NUnit.Framework;
using UnityEngine;

namespace AssemblyCSharp {

	[TestFixture]
	public class TestUtils {

        [Test]
        public void Test_ForceInInterval1() {
            Assert.AreEqual(0, MathUtils.forceInInterval(0, -1, 1));
        }
        
        [Test]
        public void Test_ForceInInterval2() {
            Assert.AreEqual(2, MathUtils.forceInInterval(0, 2, 3));
        }
        
        [Test]
        public void Test_ForceInInterval3() {
            Assert.AreEqual(-2, MathUtils.forceInInterval(0, -3, -2));
        }
        
        [Test]
        public void Test_ForceInInterval4() {
            Assert.AreEqual(34.1f, MathUtils.forceInInterval(34.1f, -9, 54f));
        }

        [Test]
        public void Test_CopyVector() {
            Vector3 v = new Vector3(1, 2, 3.4f);
            Assert.AreEqual(v, MathUtils.copy(v));
        }
        
        [Test]
        public void Test_CopyQuaternion() {
            Quaternion v = new Quaternion(1, 2, 3.4f, -1);
            Assert.AreEqual(v, MathUtils.copy(v));
        }
	}
}
