using Cars;
using Controllers;
using System.Collections.Generic;

public class Game
{

    public SpawnController SpawnController { get; set; }
    public AvailabilityController AvailabilityController { get; set; }
    public StartController StartController { get; set; }
    public List<Car> Cars { get; set; }

    public Game()
    {
        SpawnController = new SpawnController();
        AvailabilityController = new AvailabilityController();
        StartController = new StartController();
        Cars = new List<Car>();
    }

    public void AddCar(Car car)
    {
        Cars.Add(car);
        car.CarObject.setCarNumber(Cars.Count - 1);
    }
}
