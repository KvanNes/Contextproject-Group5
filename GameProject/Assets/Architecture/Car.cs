using UnityEngine;
using System;

public class Car
{

    // Players for the car variables
    public Player Driver { get; set; }
    public Player Throttler { get; set; }

    // The Unity variables
    public AutoBehaviour CarObject { get; set; }

    // Variables
    private int amountPlayers = 0;

    private static int carNumberGenerator = 0;
    public int carNumber;

    /**
     * The constructor for the car
     */
    public Car()
    {
        this.CarObject = null;
        this.carNumber = carNumberGenerator++;
    }


    /**
     * The constructor for the car
     */
    public Car(AutoBehaviour game_object)
    {
        // The car may not have more than the maximum allowed players.
        if (amountPlayers > GameData.MAX_PLAYERS_PER_CAR)
        {
            throw new UnityException(GameData.ERROR_AMOUNT_PLAYERS);
        }

        this.CarObject = game_object;
	}
	
	private bool shouldSend() {
		return MainScript.selfCar == this
			&& Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork
				&& CarObject.initialized == (AutoBehaviour.POSITION_INITIALIZED | AutoBehaviour.ROTATION_INITIALIZED);
	}
	
	public void SendToOther() {
		if(shouldSend()) {
			MainScript.selfPlayer.Role.SendToOther(this);
		}
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
            if (p.Role is Driver)
            {
                Driver = p;
                Driver.Car = this;
                amountPlayers++;
            }
            else if (p.Role is Throttler)
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
