using UnityEngine;
using System;

public class Car
{

    // Players for the car variables
    public Player Driver { get; set; }
    public Player Throttler { get; set; }

    // The Unity variables
    private GameObject CarObject { get; set; }

    // Variables
    private int amountPlayers = 0;

    /**
     * The constructor for the car
     */
    public Car()
    {
        this.CarObject = null;
    }


    /**
     * The constructor for the car
     */
    public Car(GameObject game_object)
    {
        // The car may not have more than the maximum allowed players.
        if (amountPlayers > GameData.MAX_PLAYERS_PER_CAR)
        {
            throw new UnityException(GameData.ERROR_AMOUNT_PLAYERS);
        }

        this.CarObject = game_object;
    }

    /**
     * Adds a player to the car depending on his/her function.
     * 
     * @param p      The player to add to the car.
     *  
     */
    public void addPlayer(Player p)
    {
        // The amount of players should not exceed the maximum allowed.
        if (amountPlayers > GameData.MAX_PLAYERS_PER_CAR)
        {
            System.Console.WriteLine(GameData.ERROR_AMOUNT_PLAYERS);
            return;
        }
        if (p != null)
        {
            if (p is Driver)
            {
                Driver = p;
                Driver.Car = this;
                amountPlayers++;
            }
            else if (p is Throttler)
            {
                Throttler = p;
                Throttler.Car = this;
                amountPlayers++;
            }
            else
            {
                throw new UnityException("Failsafe: The player is not a driver or throttler!");
            }
        }
    }

}
