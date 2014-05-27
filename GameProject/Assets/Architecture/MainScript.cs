using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MainScript : MonoBehaviour {

    public enum PlayerType {
        Server,
        Client,
        None
    };
    
    public static NetworkManager networkManager;
    public static List<Car> cars;
    public static Player selfPlayer;
    public static Car selfCar;
    public static PlayerType selfType = PlayerType.None;
    public static bool selectionIsFinal = false;

	// Use this for initialization
    void Start () {
        Application.runInBackground = true;

        InvokeRepeating("SendToOther", GameData.UPDATE_TIME_DELTA, GameData.UPDATE_TIME_DELTA);

        cars = new List<Car>();
        for (int i = 0; i < GameData.CARS_AMOUNT; i++) {
            cars.Add(new Car());
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void SendToOther() {
        if (selfCar != null) {
            selfCar.SendToOther();
        }
    }
}
