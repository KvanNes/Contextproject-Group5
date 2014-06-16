using Cars;
using Controllers;
using System.Collections.Generic;

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
}
