using UnityEngine;
using NUnit.Framework;
using Moq;
using System;

[TestFixture]
public class ServerTest
{
    public Server testServer;
    public Mock<INetwork> network;

    /*
     * Setup for the tests by creating the appropiate mocks and setting up the server
    */
    [SetUp]
    public void SetupServer()
    {
        GameObject gameObject = new GameObject("Server");
        testServer = gameObject.AddComponent<Server>(); // new Server();
        network = new Mock<INetwork>();
        testServer.network = network.Object;
        testServer.startServer();
    }

    /**
     * Test rather the server has made an InitializeServer call
    */
    [Test]
    public void TestInitialization()
    {
        network.Verify(net => net.InitializeServer(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<bool>()));
    }

    [Test]
    public void TestConnected()
    {

        Assert.IsTrue(testServer.isConnected());
    }

    [Test]
    public void TestDisconnect()
    {
        testServer.DisconnectServer();
        Assert.IsFalse(testServer.isConnected());
    }
}