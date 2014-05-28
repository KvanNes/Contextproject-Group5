using UnityEngine;
using System;

public class Car {

    // Players for the car variables
    public Player Driver { get; set; }
    public Player Throttler { get; set; }

    // The Unity variables
    public AutoBehaviour CarObject { get; set; }

    // Variables
    private int amountPlayers = 0;

    private static int carNumberGenerator = 0;
    public int carNumber;

    public Car(int carNumber) {
        this.carNumber = carNumber;
    }

    public Car() {
        this.carNumber = ++carNumberGenerator;  // Start with 1.
    }

    public Car(AutoBehaviour game_object) {
        // The car may not have more than the maximum allowed players.
        if (amountPlayers > GameData.MAX_PLAYERS_PER_CAR) {
            throw new UnityException(GameData.ERROR_AMOUNT_PLAYERS);
        }

        this.CarObject = game_object;
	}
	
	private bool shouldSend() {
		return CarObject != null
            && MainScript.selfCar == this
			&& Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork
			&& CarObject.initialized == (AutoBehaviour.POSITION_INITIALIZED | AutoBehaviour.ROTATION_INITIALIZED);
	}
	
	public void SendToOther() {
		if(shouldSend()) {
			MainScript.selfPlayer.Role.SendToOther(this);
		}
	}

    public void addPlayer(Player p) {
        // The amount of players should not exceed the maximum allowed.
        if (amountPlayers >= GameData.MAX_PLAYERS_PER_CAR) {
            throw new UnityException(GameData.ERROR_AMOUNT_PLAYERS);
        }

        if (p != null) {
            if (p.Role is Driver) {
                Driver = p;
                Driver.Car = this;
                ++amountPlayers;
            } else if (p.Role is Throttler) {
                Throttler = p;
                Throttler.Car = this;
                ++amountPlayers;
            } else {
                throw new UnityException("Failsafe: The player is not a driver or throttler!");
            }
        }
    }

    public int getAmountPlayers()
    {
        return amountPlayers;
    }
}
