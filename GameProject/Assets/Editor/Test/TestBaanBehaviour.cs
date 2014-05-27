using System;
using NUnit.Framework;
using UnityEngine;

namespace AssemblyCSharp {
    
    [TestFixture]
    public class TestBaanBehaviour {
        
        [Test]
        public void Test_MoveCenter1() {
            Assert.AreEqual(new Vector2[] {}, BaanBehaviour.moveCenter(new Vector2[] {}));
        }

        [Test]
        public void Test_MoveCenter2() {
            Vector2[] input = { new Vector2(1, 2) };
            Assert.AreNotEqual(input, BaanBehaviour.moveCenter(input));
        }
    }
}
