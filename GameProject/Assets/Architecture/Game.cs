using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

public class Game {

    public SpawnController SpawnController { get; set; }
    public AvailabilityController AvailabilityController { get; set; }
    public StartController StartController { get; set; }
    public List<Car> Cars { get; set; }

    public Game() {
        this.SpawnController = new SpawnController();
        this.AvailabilityController = new AvailabilityController();
        this.StartController = new StartController();
        this.Cars = new List<Car>();
    }

    public void addCar(Car car) {
        Cars.Add(car);
        car.CarObject.setCarNumber(Cars.Count - 1);
    }
}
