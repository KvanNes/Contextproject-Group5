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

public class Player {

	public Car Car { get; set; }

    public NetworkPlayer NetworkPlayer { get; set; }
	
	public PlayerRole Role { get; set; }

	public Player(Car car, PlayerRole role) {
		this.Car = car;
		this.Role = role;
	}
}
