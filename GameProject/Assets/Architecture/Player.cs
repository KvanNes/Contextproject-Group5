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

public abstract class Player
{

    // VARIABLES
    public Car Car { get; set; }

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

    public abstract void SendToOther();

    public virtual PlayerAction GetPlayerAction() {
        return PlayerAction.None;
    }

    public abstract void HandlePlayerAction(AutoBehaviour ab);

    public virtual void HandleCollision(AutoBehaviour ab) {}
}
