using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

public class Game
{

    // NETWORK MANAGER
    public NetworkView NetworkView = null;

    // VARIABLES
    private int maxCars;
    private int maxTime;
    private ArrayList cars;

    public Game(int max_cars, int max_time, NetworkView networkView)
    {
        this.maxCars = max_cars;
        this.maxTime = max_time;
        this.cars = new ArrayList();
        this.NetworkView = networkView;

        test();
    }

    public void test()
    {
        NetworkView.RPC(GameData.MESSAGE_AMOUNT_PLAYERS, RPCMode.Server, "connected");
    }

    /**
     * Creates the cars needed for the game.
     */
    public void createCars()
    {
        for (int i = 0; i < maxCars; i++)
        {
            Car c = new Car();
            addCar(c);
        }
    }

    /**
     * Method to add a car to the game.
     * 
     * @param c     The car to be added.
     */
    public void addCar(Car c)
    {
        cars.Add(c);
    }

}
