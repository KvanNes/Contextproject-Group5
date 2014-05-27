using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using System;

public class Client : MonoBehaviour {
    public Player Player { get; set; }
    public Car Car { get; set; }
    
    private string pendingType = "";
    private int pendingCarNumber = -1;
    public void chooseJobWhenConnected(string typeString, int carNumber) {
        pendingType = typeString;
        pendingCarNumber = carNumber;
    }
    
    public void OnConnectedToServer() {
        this.networkView.RPC("chooseJob", RPCMode.Server, pendingType, pendingCarNumber);
    }
    
    public void OnDisconnectedFromServer() {
        foreach(GameObject go in GameObject.FindGameObjectsWithTag("Player")) {
            Destroy(go);
        }
        NetworkController.connected = false;
        MainScript.selfCar = null;
        MainScript.selfPlayer = null;
        MainScript.selfType = MainScript.PlayerType.None;
        MainScript.selectionIsFinal = false;
    }
    
    private AutoBehaviour GetCarObjectByNumber(int carNumber) {
        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject gameObject in gameObjects) {
            AutoBehaviour ab = (AutoBehaviour) gameObject.GetComponent(typeof(AutoBehaviour));
            if(ab.carNumber == carNumber) {
                return ab;
            }
        }
        return null;
    }
    
    [RPC]
    public void chooseJobAvailable() {
        MainScript.selfCar.CarObject = GetCarObjectByNumber(MainScript.selfCar.carNumber);
        MainScript.selectionIsFinal = true;
    }
    
    [RPC]
    public void chooseJobNotAvailable() {
        Network.Disconnect();
    }
}
