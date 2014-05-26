using System;
using NUnit.Framework;
using UnityEngine;

namespace AssemblyCSharp {

	[TestFixture]
	public class TestAutoBehaviour {

        [Test]
        public void Test_NormalizeAngle1() {
            Assert.AreEqual(0, AutoBehaviour.normalizeAngle(0));
        }

        [Test]
        public void Test_NormalizeAngle2() {
            Assert.AreEqual(-90, AutoBehaviour.normalizeAngle(270));
        }
        
        [Test]
        public void Test_NormalizeAngle3() {
            Assert.AreEqual(0, AutoBehaviour.normalizeAngle(720));
        }
        
        [Test]
        public void Test_NormalizeAngle4() {
            Assert.AreEqual(180, AutoBehaviour.normalizeAngle(180));
        }
        
        [Test]
        public void Test_NormalizeAngle5() {
            Assert.AreEqual(-179, AutoBehaviour.normalizeAngle(181));
        }

        [Test]
        public void Test_ForceInInterval1() {
            Assert.AreEqual(0, AutoBehaviour.forceInInterval(0, -1, 1));
        }
        
        [Test]
        public void Test_ForceInInterval2() {
            Assert.AreEqual(2, AutoBehaviour.forceInInterval(0, 2, 3));
        }
        
        [Test]
        public void Test_ForceInInterval3() {
            Assert.AreEqual(-2, AutoBehaviour.forceInInterval(0, -3, -2));
        }
        
        [Test]
        public void Test_ForceInInterval4() {
            Assert.AreEqual(34.1f, AutoBehaviour.forceInInterval(34.1f, -9, 54f));
        }

        [Test]
        public void Test_CopyVector() {
            Vector3 v = new Vector3(1, 2, 3.4f);
            Assert.AreEqual(v, AutoBehaviour.copy(v));
        }
        
        [Test]
        public void Test_CopyQuaternion() {
            Quaternion v = new Quaternion(1, 2, 3.4f, -1);
            Assert.AreEqual(v, AutoBehaviour.copy(v));
        }
	}
}
