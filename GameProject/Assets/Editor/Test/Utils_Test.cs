using NUnit.Framework;
using UnityEngine;

[TestFixture]
public class UtilsTest
{

    [Test]
    public void Test_ForceInInterval1()
    {
        Assert.AreEqual(0, Utils.ForceInInterval(0, -1, 1));
    }

    [Test]
    public void Test_ForceInInterval2()
    {
        Assert.AreEqual(2, Utils.ForceInInterval(0, 2, 3));
    }

    [Test]
    public void Test_ForceInInterval3()
    {
        Assert.AreEqual(-2, Utils.ForceInInterval(0, -3, -2));
    }

    [Test]
    public void Test_ForceInInterval4()
    {
        Assert.AreEqual(34.1f, Utils.ForceInInterval(34.1f, -9, 54f));
    }

    [Test]
    public void Test_CopyVector()
    {
        Vector3 v = new Vector3(1, 2, 3.4f);
        Assert.AreEqual(v, Utils.Copy(v));
    }

    [Test]
    public void Test_CopyQuaternion()
    {
        Quaternion v = new Quaternion(1, 2, 3.4f, -1);
        Assert.AreEqual(v, Utils.Copy(v));
    }
}
