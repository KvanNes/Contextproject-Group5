using Cars;
using System.Collections.Generic;
using NetworkManager;
using UnityEngine;

public class Game
{
    public List<Car> Cars { get; set; }

    public Game()
    {
        Cars = new List<Car>();
    }

    public void AddCar(Car car)
    {
        Cars.Add(car);
        car.CarObject.setCarNumber(Cars.Count - 1);
    }

    public void ResetGame()
    {
        List<Car> cars = MainScript.Cars;
        Transform spawnObject = (Transform)GameObject.Find("SpawnPositionBase").GetComponent("Transform");
        foreach (Car car in cars)
        {
            float yPos = Server.GetStartingPosition(car.CarNumber);
            Vector3 resetPos = spawnObject.position + new Vector3(0, yPos, 0);
            car.CarObject.transform.position = resetPos;
            car.ResetCar(resetPos);
            car.CarObject.NetworkView.RPC("ResetCar", RPCMode.All);
            MainScript.NetworkController.NetworkView.RPC("RestartGame", RPCMode.All);
        }
    }
}
