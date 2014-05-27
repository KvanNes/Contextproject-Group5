using UnityEngine;
using System;
using System.Collections.Generic;
using System.Text;

public enum PlayerAction {
    speedUp,
    speedDown,
    steerLeft,
    steerRight,
    None
};

public class Player
{

    // VARIABLES
	public Car Car { get; set; }
	
	public PlayerRole Role { get; set; }

	public Player(Car car, PlayerRole role) {
		this.Car = car;
		this.Role = role;
	}

    /**
     * Returns the function of the current player as type of class.
     */
    public Type getFunction()
    {
        return this.GetType();
    }

    /**
     * Returns the function of the current player as a String.
     */
    public string getFunctionName()
    {
        return this.GetType().Name;
    }
}
