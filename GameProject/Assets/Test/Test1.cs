using System;
using NUnit.Framework;
using UnityEngine;

namespace AssemblyCSharp {

	[TestFixture]
	public class Test1 {

		[Test]
		public void TestA() {
			Assert.True(true);
		}

        [Test]
        public void TestB() {
            Assert.False(true);
        }
	}
}
